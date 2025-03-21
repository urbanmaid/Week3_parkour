using UnityEngine;

public class PlayerState: MonoBehaviour
{
    [Header("Control")]
    public float moveSpeed = 10f;
    public float moveSpeedMax = 14f;
    public float moveSpeedAfterLand = 7.5f;
    public float moveSpeedStunned = 3.5f;
    public float _moveSpeedCur;  // ���� private
    public float _moveTimeCur;  // ���� private
    public float _moveTimeMax = 2f;
    public bool isAccelerating = true;
    public float jumpPower = 140f;
    public float crouchPower = 6f;
    public int jumpAmount = 1;
    public int _jumpAmountCur; // ���� private
    public float fallMultiplier = 1.125f;
    public Vector3 _wallKickDirection = new(2.4f, 1.68f, 0f); // ���� private
    public bool _isRiskyToLand = false; // ���� private
    public bool _isUsingRigidbody; // ���� private

    public Vector3 _movement; // ���� private
    public Vector2 _moveInput; // ���� private

    [Header("Anim")]
    public int _wallKickStatus = 0;
    public bool _isCrouching = false; // ���� private


    [Header("Sensing - Action")]
    public float _colliderHeight; // ���� private
    public float _colliderHeightOnCrouch = 0.94f; // ���� private
    public TriggerListener triggerFeet;
    public TriggerListener triggerToe;
    public TriggerListener triggerShoulderL;
    public TriggerListener triggerShoulderR;
    public TriggerListener triggerKnee;
    public TriggerListener triggerSternum;
    public CapsuleCollider capsuleCollider;

    [Header("Sensing - Collision")]
    public TriggerListener triggerCollisionKnee; // Message when collided on low obstacle
    public TriggerListener triggerCollisionSternum; // Message when collided on high obstacle


    [Header("Expression")]
    public RobotAnimator robotAnimator;


}
