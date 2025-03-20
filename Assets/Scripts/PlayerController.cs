using System.Collections;
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
    [SerializeField] TriggerListener triggerKnee;
    [SerializeField] TriggerListener triggerSternum;
    [SerializeField] CapsuleCollider capsuleCollider;
    private float _colliderHeight;
    private float _colliderHeightOnCrouch = 0.94f;
    [SerializeField] RobotAnimator robotAnimator;

    [Header("Sensing - Collision")]
    [SerializeField] TriggerListener triggerCollisionKnee; // Message when collided on low obstacle
    [SerializeField] TriggerListener triggerCollisionSternum; // Message when collided on high obstacle

    [Header("Control")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float moveSpeedMax = 14f;
    [SerializeField] float moveSpeedAfterLand = 7.5f;
    [SerializeField] float moveSpeedStunned = 3.5f;
    private float _moveSpeedCur;
    private float _moveTimeCur;
    [SerializeField] float _moveTimeMax = 2f;
    [SerializeField] bool isAccelerating = true;
    [SerializeField] float jumpPower = 24f;
    [SerializeField] float crouchPower = 6f;
    [SerializeField] int jumpAmount = 1;
    [SerializeField] int _jumpAmountCur;
    private Vector3 _wallKickDirection = new(2.4f, 1.75f, 0f);
    private bool _isRiskyToLand = false;
    private bool _isUsingRigidbody;
    private InputActions _inputActions;
    private Vector3 _movement;
    private Vector2 _moveInput;

    [Header("Anim")]
    private int _wallKickStatus = 0;

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

    void OnEnable()
    {
        _inputActions.Player.Enable(); // Player Action Map 활성화
    }

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
        if(rb.linearVelocity.y < -10f)
        {
            _isRiskyToLand = true;
        }
        else
        {
            _isRiskyToLand = false;
        }
    }
    #endregion

    #region Move
    void Move()
    {
        _movement = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized * _moveSpeedCur;
        if(!_isUsingRigidbody)
        {
            // If player tend to stay in wall, make it unable
            if((triggerShoulderL.isTriggered && _moveInput.x > 0f)
             ||(triggerShoulderR.isTriggered && _moveInput.x < 0f))
            {
                _movement.x = 0f;
            }
            if(triggerToe.isTriggered && _moveInput.y < 0f)
            {
                _movement.z = 0f;
            }

            // Move with linearVelocity
            rb.linearVelocity = new Vector3(_movement.x, rb.linearVelocity.y, _movement.z); // Y축은 점프에만 영향
        }

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

    void SetAnim()
    {
        if(_movement.magnitude < 0.08f)
        {
            if(_wallKickStatus == -1)
            {
                robotAnimator.transform.rotation = Quaternion.LookRotation(Vector3.left);
                robotAnimator.SetWallKick();
            }
            else if(_wallKickStatus == 1)
            {
                robotAnimator.transform.rotation = Quaternion.LookRotation(Vector3.right);
                robotAnimator.SetWallKick();
            }
            else{
                robotAnimator.SetIdle();
            }
        }
        else
        {
            if(_isUsingRigidbody)
            {
                if(_wallKickStatus == -1)
                {
                    robotAnimator.transform.rotation = Quaternion.LookRotation(Vector3.left);
                    robotAnimator.SetWallKick();
                }
                else if(_wallKickStatus == 1)
                {
                    robotAnimator.transform.rotation = Quaternion.LookRotation(Vector3.right);
                    robotAnimator.SetWallKick();
                }
            }
            else
            {
                if(_jumpAmountCur != jumpAmount)
                {
                    robotAnimator.SetJump();
                }
                else
                {
                    robotAnimator.SetRun();
                }

                // Set Avatar rotation
                robotAnimator.transform.rotation = Quaternion.LookRotation(new Vector3(_movement.x, 0f, _movement.z).normalized);
            }
        }        
    }

    #endregion

    #region Jump - Start
    void Jump()
    {
        if(_jumpAmountCur > 0) // If on the ground
        {
            _jumpAmountCur--;
            if(triggerSternum.isTriggered) // 3m Jump should be done after jump
            {
                StartCoroutine(CrossObstacleHigh());
            }
            else
            {
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
            else if(triggerKnee.isTriggered) // If player is only hanging on the wall
            {
                StartCoroutine(CrossObstacleHigh());
            }
            else if(triggerFeet.isTriggered) // But not certain that it is on the airtime so resets the status
            {
                Debug.LogWarning("Has some issues while jump amount is not charged even on the ground");
                //SetJumpPower();
                _jumpAmountCur = jumpAmount;
                _isUsingRigidbody = false;
            }
        }
    }
    void SetJumpPower()
    {
        float jumpPowerCur = jumpPower + (_moveTimeCur * jumpPower * 0.25f);
        _moveTimeCur = 0;
        //Debug.Log("Jump Power : " + jumpPowerCur);
        rb.AddForce(_movement, ForceMode.Acceleration);
        rb.AddForce(Vector3.up * jumpPowerCur, ForceMode.Impulse);
    }
    /*
    IEnumerator CrossObstacle()
    {
        _isUsingRigidbody = true;
        // Step 1, let rigidbody control my body and set force
        rb.AddForce(new Vector3(0f, 1f * jumpPower, 0f), ForceMode.Impulse);
        Debug.Log("Crossing Obstacle");

        // Step 2, go forward
        yield return new WaitForSeconds(0.25f);
        rb.AddForce(new Vector3(0f, 0f, 1.6f * crouchPower), ForceMode.Impulse);

        // Step 3, return to basis
        yield return new WaitForSeconds(0.15f);
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(new Vector3(0f, -1f * crouchPower, 0f), ForceMode.Impulse);
    }
    */
    IEnumerator CrossObstacleHigh()
    {
        _isUsingRigidbody = true;
        rb.AddForce(new Vector3(0f, 2f * jumpPower, 0f), ForceMode.Impulse);
        Debug.Log("2m Obstacle has been set");
        
        yield return new WaitForSeconds(0.5f);
        _isUsingRigidbody = false;
        rb.AddForce(new Vector3(0f, 0f, 2f * crouchPower), ForceMode.Impulse);
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
        
        if(_isRiskyToLand)
        {
            _moveSpeedCur = moveSpeedStunned;
            isAccelerating = false;
            Debug.LogWarning("You are too fast to land off without injury");

            yield return new WaitForSeconds(0.75f);
            //SetAcceleratingOn();
        }
        else
        {
            _moveSpeedCur = moveSpeedAfterLand;
            isAccelerating = false;
            SetColliderCrouch();
            //Debug.Log("You have landed");

            yield return new WaitForSeconds(0.4f);
            //SetAcceleratingOn();
            ResetColliderCrouch();
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
        rb.AddForce(jumpPower * Vector3.Scale(_wallKickDirection, new Vector3(-1f, 1f, 1f)), ForceMode.Impulse);
    }
    void WallKickR()
    {
        _isUsingRigidbody = true;
        _wallKickStatus = 1;
        rb.AddForce(jumpPower * _wallKickDirection, ForceMode.Impulse);
    }

    #endregion

    #region Crouch
    // Fix Log #1
    IEnumerator Crouch()
    {
        if(_jumpAmountCur > 0 && _moveInput.magnitude > 0.08f)
        {
            // Reset Crouch Status
            _isUsingRigidbody = true;
            rb.AddForce(crouchPower * new Vector3(_movement.x, rb.linearVelocity.y / crouchPower, _movement.z), ForceMode.Impulse);
            SetColliderCrouch();

            // Reset Crouch Status
            yield return new WaitForSeconds(0.5f);
            _isUsingRigidbody = false;
            ResetColliderCrouch();
        }
    }
    void SetColliderCrouch()
    {
        capsuleCollider.height = _colliderHeightOnCrouch;
        capsuleCollider.center = new Vector3(0f, 0.5f * _colliderHeightOnCrouch, 0f);
    }
    void ResetColliderCrouch()
    {
        capsuleCollider.height = _colliderHeight;
        capsuleCollider.center = new Vector3(0f, 0.5f * _colliderHeight, 0f);
    }
    #endregion

    #region Achievement
    public void SetCollisionLow()
    {
        if(capsuleCollider.height == _colliderHeight)
        {
            AchievementManager.instance.UpdateCollisionLow();
        }
    }
    public void SetCollisionHigh()
    {
        if(capsuleCollider.height == _colliderHeight)
        {
            if((rb.linearVelocity.y > -0.02f || 0.02f > rb.linearVelocity.y)
            && triggerFeet.isTriggered) // If player is on the ground and velocity axis y is nearly 0
            {
                AchievementManager.instance.UpdateCollisionHigh();
            }
        }
    }
    void SetAccumulatedDist()
    {
        AchievementManager.instance.UpdateDist(Mathf.Round(rb.linearVelocity.magnitude * Time.deltaTime * 100f) / 100f);
    }
    #endregion
}
