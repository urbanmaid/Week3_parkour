
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private Rigidbody rb;
    [Header("Sensing - Action")]
    [SerializeField] TriggerListener triggerFeet;
    [SerializeField] TriggerListener triggerToe;
    [SerializeField] TriggerListener triggerShoulderL;
    [SerializeField] TriggerListener triggerShoulderR;
    [SerializeField] TriggerListener triggerCollision;
    [SerializeField] CapsuleCollider capsuleCollider;
    private float _colliderHeight;
    private float _colliderHeightOnCrouch = 0.94f;

    [Header("Expression")]
    public RobotAnimator robotAnimator;
    public ParticleSystem jumpFXParticle;

    [Header("Control")]
    [SerializeField] float moveSpeed = 24f;
    [SerializeField] float moveSpeedHorizonal = 15f;
    [SerializeField] float moveSpeedMax = 28f;
    [SerializeField] float moveSpeedAfterLand = 15f;
    [SerializeField] float moveSpeedStunned = 4f;
    private float _moveSpeedCur;
    private float _moveTimeCur;
    [SerializeField] float _moveTimeMax = 2f;
    [SerializeField] bool isAccelerating = true;
    [SerializeField] float jumpPower = 24f;
    [SerializeField] float crouchPower = 6f;
    [SerializeField] int jumpAmount = 1;
    private int _jumpAmountCur;
    [SerializeField] float fallMultiplier = 1.125f;
    private Vector3 _wallKickDirection = new(2.4f, 1.68f, 0f);
    private bool _isRiskyToLand = false;
    private bool _isUsingRigidbody;
    private InputActions _inputActions;
    private Vector3 _movement;
    private Vector3 _movementLerp;
    [SerializeField] Vector2 _moveInput;
    private readonly float lerpDelay = 10f;


    [Header("Anim")]
    private int _wallKickStatus = 0;
    private bool _isCrouching = false;
    private bool _isCollided = false;
    [SerializeField] CameraFX playerCamera;
    [SerializeField] PlayerFollower playerFollower;

    #endregion

    #region Start
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AssignComponent();
        _jumpAmountCur = jumpAmount;
        _moveSpeedCur = moveSpeed;

        //Control setting
        AssignControl();
    }

    void AssignComponent()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        _colliderHeight = capsuleCollider.height;
    }

    /*
    void OnEnable()
    {
        _inputActions.Player.Enable(); // Player Action Map 활성화
    }
    */

    void OnDisable()
    {
        _inputActions.Player.Disable(); // 비활성화 시 입력 끄기
    }

    #endregion

    #region Control
    // Update is called once per frame
    internal void DoUpdate()
    {
        Move();
        SetMoveSpeed();
        CheckFallenSpeed();

        SetAnim();

        playerCamera.UpdateCamera();
        if(playerFollower)
        {
            playerFollower.DoUpdate();
        }
    }

    void AssignControl()
    {
        _inputActions = new InputActions();
        if(_inputActions != null)
        {
            Debug.Log("inputActions has been loaded");
        }
        _inputActions.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _inputActions.Player.Move.canceled += ctx => _moveInput = Vector2.zero;
        _inputActions.Player.Jump.performed += ctx => Jump();
        _inputActions.Player.Crouch.performed += ctx => StartCoroutine(Crouch());

        _inputActions.Player.Enable();
    }
    void CheckFallenSpeed()
    {
        if(rb.linearVelocity.y < -18f)
        {
            _isRiskyToLand = true;
        }
        else
        {
            _isRiskyToLand = false;
        }

        if (rb.linearVelocity.y < -0.02f) // 떨어질 때
        {
            rb.linearVelocity += (fallMultiplier - 1) * Physics.gravity.y * Time.fixedDeltaTime * Vector3.up;
        }
    }
    #endregion

    #region Move
    void Move()
    {
        // Only Z Axis(Front-rear can be applied its accelation)
        _movement =  Vector3.Scale(new Vector3(_moveInput.x, 0f, _moveInput.y), new Vector3(moveSpeedHorizonal, 1f, _moveSpeedCur));
        _movementLerp = Vector3.Lerp(_movementLerp, _movement, Time.deltaTime * lerpDelay);

        if(!_isUsingRigidbody && !_isCollided)
        {
            // If player tend to stay in wall, make it unable
            if((triggerShoulderL.isTriggered && _movementLerp.x > 0f)
             ||(triggerShoulderR.isTriggered && _movementLerp.x < 0f))
            {
                _movementLerp.x = 0f;
            }
            if(triggerToe.isTriggered && _movementLerp.y > 0f)
            {
                _movementLerp.z = 0f;
            }

            // Move with linearVelocity
            rb.linearVelocity = new Vector3(_movementLerp.x, rb.linearVelocity.y, _movementLerp.z); // Y축은 점프에만 영향
        }

        // Check is moveable
        //if(!triggerFeet.isTriggered)

        // Check move duration
        if(_movement.magnitude < 0.08f)
        {
            _moveTimeCur = 0;
        }
        else
        {
            SetAccumulatedDist();
            if(_moveTimeMax > _moveTimeCur)
            {
                _moveTimeCur += Time.deltaTime;
            }
        }
    }
    void SetMoveSpeed()
    {
        if(isAccelerating)
        {
            _moveSpeedCur = moveSpeed + ((moveSpeedMax - moveSpeed) * (_moveTimeCur / _moveTimeMax));
        }
    }
    #endregion

    #region Jump - Start
    void Jump()
    {
        if(_jumpAmountCur > 0) // If on the ground
        {
            if(triggerFeet.isTriggered)
            {
                _jumpAmountCur--;
                if(_isCrouching)
                {
                    //ResetColliderCrouch();
                    _isUsingRigidbody = false;
                    _isCrouching = false;
                }
                SetJumpPower();
            }

        }
        else if(_jumpAmountCur == 0) // If on the airtime
        {
            if(triggerShoulderL.isTriggered)
            {
                _moveTimeCur = 0;
                rb.linearVelocity = Vector3.zero;
                WallKickL();
            }
            else if(triggerShoulderR.isTriggered)
            {
                _moveTimeCur = 0;
                rb.linearVelocity = Vector3.zero;
                WallKickR();
            }
            else if(triggerFeet.isTriggered) // But not certain that it is on the airtime so resets the status
            {
                Debug.LogWarning("Has some issues while jump amount is not charged even on the ground");
                _jumpAmountCur = jumpAmount;
                //_isUsingRigidbody = false;
            }
        }
    }
    void SetJumpPower()
    {
        // Calculate jump power
        //_isUsingRigidbody = true;
        float jumpPowerCur = jumpPower + (_moveTimeCur * jumpPower * 0.05f);
        _moveTimeCur = 0f;

        // Set all the jump pattern should be Impulse
        Vector3 jumpForce = (_movement.normalized + Vector3.up) * jumpPowerCur;
        rb.AddForce(jumpForce, ForceMode.Impulse);

        StartCoroutine(ShowJumpFX());
    }
    #endregion

    #region Jump - Med
    public void ResetJumpStatus()
    {
        StartCoroutine(LandCo());
    }
    IEnumerator LandCo()
    {
        _jumpAmountCur = jumpAmount;
        _isUsingRigidbody = false;
        _wallKickStatus = 0;

        // Add playerCamera push into original place
        playerCamera.ResetFOV();
        
        if(_isRiskyToLand)
        {
            _moveSpeedCur = moveSpeedStunned;
            isAccelerating = false;
            Debug.LogWarning("You are too fast to land off without injury");
            // Set playerCamera shakes alot
            StartCoroutine(playerCamera.ApplyOffsetFXDamage2());

            yield return new WaitForSeconds(0.75f);
        }
        else
        {
            _moveSpeedCur = moveSpeedAfterLand;
            isAccelerating = false;
            // Set playerCamera shakes little

            yield return new WaitForSeconds(0.3f);
        }
        SetAcceleratingOn();
    }
    public void SetRestrictedLand()
    {
        rb.linearVelocity = Vector3.zero;
        Debug.LogWarning("You have landed area with restricted jump");
    }

    void SetAcceleratingOn()
    {
        _moveSpeedCur = moveSpeed;
        _moveTimeCur = 0;
        isAccelerating = true;
    }
    #endregion

    #region Wallkick
    void WallKickL()
    {
        _isUsingRigidbody = true;
        _wallKickStatus = -1;

        // Add playerCamera pullup
        playerCamera.SetFOVZoomOut();

        rb.AddForce(jumpPower * Vector3.Scale(_wallKickDirection, new Vector3(-1f, 1f, 1f)), ForceMode.Impulse);

        StartCoroutine(ShowJumpFX());
    }
    void WallKickR()
    {
        _isUsingRigidbody = true;
        _wallKickStatus = 1;

        // Add playerCamera pullup
        playerCamera.SetFOVZoomOut();

        rb.AddForce(jumpPower * _wallKickDirection, ForceMode.Impulse);
        
        StartCoroutine(ShowJumpFX());
    }
    public void SetWallKickPrep(int value)
    {
        if(_jumpAmountCur != jumpAmount)
        {
            _wallKickStatus = value;
        }
    }
    #endregion

    #region Crouch
    IEnumerator Crouch()
    {
        if(_jumpAmountCur > 0 && _moveInput.magnitude > 0.08f && !_isCrouching
        && _moveInput.y > 0.88f && !(Math.Abs(_moveInput.x) > 0.88f && _moveInput.y < 0.88f))
        // Crouch should be done when is on the ground, moving, not crouching
        // and its moving direction is not diagonal
        {
            _isUsingRigidbody = true;
            rb.AddForce(crouchPower * new Vector3(_movement.x, rb.linearVelocity.y / crouchPower, _movement.z), ForceMode.Impulse);
            SetColliderCrouch();
            playerCamera.SetFOVZoomOut();

            // Reset Crouch Status
            yield return new WaitForSeconds(0.5f);
            _isUsingRigidbody = false;
            ResetColliderCrouch();
            playerCamera.ResetFOV();

            //After that makes it able to re-crouch after awhile
            //yield return new WaitForSeconds(0.1f);
            _isCrouching = false;
        }
    }
    void SetColliderCrouch()
    {
        // Set playerCamera FOV longer

        _isCrouching = true;
        capsuleCollider.height = _colliderHeightOnCrouch;
        capsuleCollider.center = new Vector3(0f, 0.5f * _colliderHeightOnCrouch, 0f);
    }
    void ResetColliderCrouch()
    {
        // Reset playerCamera FOV

        capsuleCollider.height = _colliderHeight;
        capsuleCollider.center = new Vector3(0f, 0.5f * _colliderHeight, 0f);
    }
    #endregion

    #region Collision
    public void SetCollided()
    {
        if(triggerCollision.isTriggered
        && (!_isCrouching)) // || jumpAmount == _jumpAmountCur
        {
            Debug.Log("Collided into obstacle");
            _isCollided = true;

            rb.linearVelocity = Vector3.zero;
            transform.Translate(Vector3.back * 1.6f);

            StartCoroutine(playerCamera.ApplyOffsetFXDamage2());
            Invoke(nameof(EndCollision), 1f);
        }
    }
    public void StopPlayer()
    {
        StartCoroutine(playerCamera.ApplyOffsetFXDamage2());

        _isUsingRigidbody = true;
        _isCollided = true;
        rb.linearVelocity = Vector3.zero;
    }
    public void SetCollidedStatus(bool value)
    {
        _isCollided = value;
    }

    void EndCollision()
    {
        //Debug.Log("Collided has ended");
        _isCollided = false;
    }

    internal void SetUsingRigidbody(bool value)
    {
        _isUsingRigidbody = value;
    }
    #endregion

    #region Achievement
    void SetAccumulatedDist()
    {
        AchievementManager.instance.UpdateDist(Mathf.Round(rb.linearVelocity.magnitude * Time.deltaTime * 100f) / 100f);
    }
    #endregion

    #region Animation
    void SetAnim()
    {
        if(robotAnimator.gameObject)
        {
            if (_movementLerp.magnitude < 0.08f)
            {
                if (_wallKickStatus == -1)
                {
                    robotAnimator.transform.rotation = Quaternion.LookRotation(Vector3.left);
                    robotAnimator.SetWallKick();
                }
                else if (_wallKickStatus == 1)
                {
                    robotAnimator.transform.rotation = Quaternion.LookRotation(Vector3.right);
                    robotAnimator.SetWallKick();
                }
                else if (_wallKickStatus == -10)
                {
                    robotAnimator.transform.rotation = Quaternion.LookRotation(Vector3.right);
                    robotAnimator.SetWallKickPrep();
                }
                else if (_wallKickStatus == 10)
                {
                    robotAnimator.transform.rotation = Quaternion.LookRotation(Vector3.left);
                    robotAnimator.SetWallKickPrep();
                }
                else
                {
                    if (_jumpAmountCur != jumpAmount)
                    {
                        robotAnimator.SetJump();
                    }
                    else
                    {
                        robotAnimator.SetJumpLand();
                        if (_moveSpeedCur == moveSpeedStunned)
                        {
                            robotAnimator.SetDamage();
                        }
                        if (_moveSpeedCur == moveSpeed)
                        {
                            robotAnimator.SetIdle();
                        }

                        if (_isCollided)
                        {
                            robotAnimator.SetDeath();
                        }
                    }
                }
            }
            else
            {
                if (_isUsingRigidbody)
                {
                    if (_isCrouching)
                    {
                        robotAnimator.SetCrouch();
                    }
                }
                else
                {
                    if (_jumpAmountCur != jumpAmount)
                    {
                        robotAnimator.SetJump();
                    }
                    else
                    {
                        if (_moveSpeedCur == moveSpeedAfterLand)
                        {
                            robotAnimator.SetJumpLand();
                        }
                        else if (_moveSpeedCur == moveSpeedStunned)
                        {
                            robotAnimator.SetDamage();
                        }
                        else
                        {
                            robotAnimator.SetRun();
                        }

                        if (_isCollided)
                        {
                            robotAnimator.SetDeath();
                        }
                    }

                    if (!_isCollided)
                    {
                        // Set Avatar rotation
                        robotAnimator.transform.rotation = Quaternion.LookRotation(new Vector3(_movementLerp.x, 0f, _movementLerp.z).normalized);
                    }
                }
            }
        }
    }

    IEnumerator ShowJumpFX()
    {
        if(jumpFXParticle)
        {
            jumpFXParticle.Play();

            yield return new WaitForSeconds(0.2f);
            jumpFXParticle.Stop();
        }
    }
    #endregion
}
