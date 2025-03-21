using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /* Fields */
    private Rigidbody _rb;

    public TriggerListener triggerFeet;
    public TriggerListener triggerToe;
    public TriggerListener triggerShoulderL;
    public TriggerListener triggerShoulderR;
    public TriggerListener triggerKnee;
    public TriggerListener triggerSternum;
    public CapsuleCollider capsuleCollider;

    public TriggerListener triggerCollisionKnee; // Message when collided on low obstacle
    public TriggerListener triggerCollisionSternum; // Message when collided on high obstacle

    public RobotAnimator robotAnimator;




    // 추가한 스크립트 컨트롤러들


    public InputManager inputManager; // 이거도 할당보다는 참조가 나을거같은데..

    private PlayerActionMove _move;
    private PlayerActionJump _jump;
    private PlayerActionWallkick _wallkick;
    private PlayerActionCrouch _crouch;

    private PlayerAchievement _achievement;
    private PlayerAnimation _animation;

    private PlayerState _state;


    /* Methods */

    private void Awake()
    {
        _state = GetComponent<PlayerState>();
        _rb = GetComponent<Rigidbody>();
        _state.robotAnimator = robotAnimator;


        inputManager.InitInputAction(this, _state);

        _move = new PlayerActionMove(this, _state, _rb);
        _jump = new PlayerActionJump(this, _state, _rb);
        _wallkick = new PlayerActionWallkick(this, _state, _rb);
        _crouch = new PlayerActionCrouch(this, _state, _rb);

        _achievement = new PlayerAchievement(this, _state, _rb);
        _animation = new PlayerAnimation(this, _state, _rb);


        

        _state.triggerFeet = triggerFeet;
        _state.triggerToe = triggerToe;
        _state.triggerShoulderL = triggerShoulderL;
        _state.triggerShoulderR = triggerShoulderR;
        _state.triggerKnee = triggerKnee;
        _state.triggerSternum = triggerSternum;
        _state.capsuleCollider = capsuleCollider;

        _state.triggerCollisionKnee = triggerCollisionKnee; // Message when collided on low obstacle
        _state.triggerCollisionSternum = triggerCollisionSternum; // Message when collided on high obstacle
    }

    private void Start()
    {
        AssignComponent();
        _state._jumpAmountCur = _state.jumpAmount;
        _state._moveSpeedCur = _state.moveSpeed;

        //_animation.SetAnim();
    }

    private void Update()
    {
        Move();
        _move.SetMoveSpeed();
        CheckFallenSpeed();
        _animation.SetAnim();
    }


    void AssignComponent()
    {
        _rb = GetComponent<Rigidbody>();
        _state.capsuleCollider = GetComponent<CapsuleCollider>();
        _state._colliderHeight = _state.capsuleCollider.height;
    }

    void CheckFallenSpeed()
    {
        if (_rb.linearVelocity.y < -25f)
        {
            _state._isRiskyToLand = true;
        }
        else
        {
            _state._isRiskyToLand = false;
        }

        if (_rb.linearVelocity.y < -0.02f) // 떨어질 때
        {
            _rb.linearVelocity += (_state.fallMultiplier - 1) * Physics.gravity.y * Time.fixedDeltaTime * Vector3.up;
        }
    }

    public void SetAccumulatedDist()
    {
        _achievement.SetAccumulatedDist();
    }



    public void Move()
    {
        _move.Move();
    }

    public void Jump()
    {
        _jump.Jump();
    }

    public void WallKickL()
    {
        _wallkick.WallKickL();
    }

    public void WallKickR()
    {
        _wallkick.WallKickR();
    }

    public IEnumerator Crouch()
    {
        return _crouch.Crouch();
    }


    public new void StartCoroutine(IEnumerator routine)
    {
        base.StartCoroutine(routine);
    }
}
