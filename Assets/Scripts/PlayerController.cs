using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private Rigidbody rb;
    [SerializeField] TriggerListener triggerFeet;
    [SerializeField] TriggerListener triggerShoulderL;
    [SerializeField] TriggerListener triggerShoulderR;
    [SerializeField] TriggerListener triggerKnee;
    [SerializeField] TriggerListener triggerSternum;
    [SerializeField] CapsuleCollider capsuleCollider;
    

    [Header("Control")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float moveSpeedMax = 14f;
    [SerializeField] float moveSpeedAfterLand = 7.5f;
    [SerializeField] float moveSpeedStunned = 3.5f;
    private float _moveSpeedCur;
    private float _moveTimeCur;
    private float _moveTimeMax = 3f;
    [SerializeField] float jumpPower = 10f;
    [SerializeField] float crouchPower = 6f;
    [SerializeField] int jumpAmount = 1;
    private int _jumpAmountCur;
    private bool _isRiskyToLand = false;
    private bool _isUsingRigidbody;
    private InputActions _inputActions;
    private Vector3 _movement;
    private Vector2 _moveInput;
    #endregion

    #region Start
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _jumpAmountCur = jumpAmount;
        _moveSpeedCur = moveSpeed;

        //Control setting
        AssignControl();
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
        CheckFallenSpeed();
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

    #region Action
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

            // Move with linearVelocity
            rb.linearVelocity = new Vector3(_movement.x, rb.linearVelocity.y, _movement.z); // Y축은 점프에만 영향
        }
    }

    void Jump()
    {
        if(_jumpAmountCur > 0)
        {
            _jumpAmountCur--;
            if(triggerKnee.isTriggered) // Obstacle Cross
            {
                CrossObstacle();
            }
            /*
            else if(triggerKnee.isTriggered && triggerSternum.isTriggered)
            {
                transform.Translate(Vector3.up * Time.deltaTime);
            }
            */
            else // Normal jump
            {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }
        else if(_jumpAmountCur == 0)
        {
            if(triggerShoulderL.isTriggered)
            {
                rb.linearVelocity = Vector3.zero;
                WallKickL();
            }
            else if(triggerShoulderR.isTriggered)
            {
                rb.linearVelocity = Vector3.zero;
                WallKickR();
            }
        }
    }
    void CrossObstacle()
    {
        _isUsingRigidbody = true;
        rb.AddForce(new Vector3(_movement.x, 1f * jumpPower, _movement.z), ForceMode.Impulse);
        Debug.Log("Crossing Obstacle");
    }
    public void ResetJumpStatus()
    {
        StartCoroutine(LandCo());
    }

    IEnumerator LandCo()
    {
        _jumpAmountCur = jumpAmount;
        _isUsingRigidbody = false;
        if(_isRiskyToLand)
        {
            _moveSpeedCur = moveSpeedStunned;
            Debug.LogWarning("You are too fast to land off without injury");

            yield return new WaitForSeconds(0.75f);
            _moveSpeedCur = moveSpeed;
        }
        else
        {
            _moveSpeedCur = moveSpeedAfterLand;
            Debug.Log("You have landed");

            yield return new WaitForSeconds(0.4f);
            _moveSpeedCur = moveSpeed;
        }
    }

    void WallKickL()
    {
        _isUsingRigidbody = true;
        rb.AddForce(jumpPower * new Vector3(-1.6f, 1f, 0f), ForceMode.Impulse);
    }

    void WallKickR()
    {
        _isUsingRigidbody = true;
        rb.AddForce(jumpPower * new Vector3(1.6f, 1f, 0f), ForceMode.Impulse);
    }

    IEnumerator Crouch()
    {
        if(_jumpAmountCur > 0)
        {
            _isUsingRigidbody = true;
            rb.AddForce(crouchPower * new Vector3(_movement.x, rb.linearVelocity.y, _movement.z), ForceMode.Impulse);

            yield return new WaitForSeconds(0.5f);
            _isUsingRigidbody = false;
        }
    }
    #endregion
}
