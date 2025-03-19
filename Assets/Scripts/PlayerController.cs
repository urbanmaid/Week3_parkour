using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] TriggerListener triggerFeet;
    [SerializeField] TriggerListener triggerShoulderL;
    [SerializeField] TriggerListener triggerShoulderR;
    [SerializeField] TriggerListener triggerKnee;
    [SerializeField] TriggerListener triggerSternum;
    [SerializeField] CapsuleCollider capsuleCollider;
    

    [Header("Control")]
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float moveSpeedStunned = 3.5f;
    [SerializeField] float moveSpeedCur;
    [SerializeField] float jumpPower = 10f;
    [SerializeField] float crouchPower = 6f;
    [SerializeField] int jumpAmount = 1;
    private int jumpAmountCur;
    private bool isRiskyToLand = false;
    private bool isUsingRigidbody;
    private InputActions inputActions;
    [SerializeField] Vector3 movement;
    [SerializeField] Vector2 moveInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpAmountCur = jumpAmount;
        moveSpeedCur = moveSpeed;

        //Control setting
        AssignControl();
    }

    void OnEnable()
    {
        inputActions.Player.Enable(); // Player Action Map 활성화
    }

    void OnDisable()
    {
        inputActions.Player.Disable(); // 비활성화 시 입력 끄기
    }

    // Update is called once per frame
    internal void Update()
    {
        Move();
        CheckFallenSpeed();
    }

    void AssignControl()
    {
        inputActions = new InputActions();
        if(inputActions != null)
        {
            Debug.Log("inputActions has been loaded");
        }
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Jump.performed += ctx => Jump();
        inputActions.Player.Crouch.performed += ctx => StartCoroutine(Crouch());

        inputActions.Player.Enable();
    }
    void CheckFallenSpeed()
    {
        if(rb.linearVelocity.y < -5f)
        {
            isRiskyToLand = true;
        }
        else
        {
            isRiskyToLand = false;
        }
    }

    void Move()
    {
        movement = new Vector3(moveInput.x, 0f, moveInput.y).normalized * moveSpeedCur;
        if(!isUsingRigidbody)
        {
            // If player tend to stay in wall, make it unable
            if((triggerShoulderL.isTriggered && moveInput.x > 0f)
             ||(triggerShoulderR.isTriggered && moveInput.x < 0f))
            {
                movement.x = 0f;
            }

            // Move with linearVelocity
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z); // Y축은 점프에만 영향
        }
    }

    void Jump()
    {
        if(jumpAmountCur > 0)
        {
            jumpAmountCur--;
            if(triggerKnee.isTriggered) // Obstacle Cross
            {
                CrossObstacle();
            }
            else if(triggerKnee.isTriggered && triggerSternum.isTriggered)
            {
                transform.Translate(Vector3.up * Time.deltaTime);
            }
            else // Normal jump
            {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }
        else if(jumpAmountCur == 0)
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
        isUsingRigidbody = true;
        rb.AddForce(new Vector3(movement.x, 1f * jumpPower, movement.z), ForceMode.Impulse);
        Debug.Log("Crossing Obstacle");
    }
    public void ResetJumpStatus()
    {
        StartCoroutine(LandCo());
    }

    IEnumerator LandCo()
    {
        jumpAmountCur = jumpAmount;
        isUsingRigidbody = false;
        if(isRiskyToLand)
        {
            moveSpeedCur = moveSpeedStunned;
            Debug.LogWarning("You are too fast to land off without injury");

            yield return new WaitForSeconds(1f);
            moveSpeedCur = moveSpeed;
        }
    }

    void WallKickL()
    {
        isUsingRigidbody = true;
        rb.AddForce(jumpPower * new Vector3(-1.6f, 1f, 0f), ForceMode.Impulse);
    }

    void WallKickR()
    {
        isUsingRigidbody = true;
        rb.AddForce(jumpPower * new Vector3(1.6f, 1f, 0f), ForceMode.Impulse);
    }

    IEnumerator Crouch()
    {
        if(jumpAmountCur > 0)
        {
            isUsingRigidbody = true;
            rb.AddForce(crouchPower * new Vector3(movement.x, rb.linearVelocity.y, movement.z), ForceMode.Impulse);

            yield return new WaitForSeconds(0.5f);
            isUsingRigidbody = false;
        }
    }


}
