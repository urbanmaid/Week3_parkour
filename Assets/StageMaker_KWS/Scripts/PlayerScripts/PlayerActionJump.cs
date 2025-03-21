using System.Collections;
using UnityEngine;

public class PlayerActionJump
{
    private PlayerController _controller;
    private PlayerState _state;
    private Rigidbody _rb;

    public PlayerActionJump(PlayerController controller, PlayerState state, Rigidbody rb)
    {
        _controller = controller;
        _state = state;
        _rb = rb;
    }


    // Start Jump region
    public void Jump()
    {
        Debug.Log($"Jump Amount cur: {_state._jumpAmountCur}");

        if (_state._jumpAmountCur > 0) // Idf on the ground
        {
            if (_state.triggerFeet.isTriggered)
            {
                _state._jumpAmountCur--;
                if (_state.triggerSternum.isTriggered) // 3m Jump should be done after jump
                {
                    _controller.StartCoroutine(CrossObstacleHigh());
                }
                else
                {
                    SetJumpPower();
                }
            }

        }
        else if (_state._jumpAmountCur == 0) // If on the airtime
        {
            Debug.Log($"Airborn, ShoulderL: {_state.triggerShoulderL.isTriggered}");

            if (_state.triggerShoulderL.isTriggered)
            {
                _state._moveTimeCur = 0;
                _rb.linearVelocity = Vector3.zero;
                _controller.WallKickL();
            }
            else if (_state.triggerShoulderR.isTriggered)
            {
                _state._moveTimeCur = 0;
                _rb.linearVelocity = Vector3.zero;
                _controller.WallKickR();
            }
            else if (_state.triggerSternum.isTriggered) // If player is only hanging on the wall
            {
                _controller.StartCoroutine(CrossObstacleHigh());
            }

            else if (_state.triggerFeet.isTriggered) // But not certain that it is on the airtime so resets the status
            {
                Debug.LogWarning("Has some issues while jump amount is not charged even on the ground");
                //SetJumpPower();
                _state._jumpAmountCur = _state.jumpAmount;
                //_isUsingRigidbody = false;
            }
        }

        void SetJumpPower()
        {
            // 점프력 계산
            _state._isUsingRigidbody = true;
            float jumpPowerCur = _state.jumpPower + (_state._moveTimeCur * _state.jumpPower * 0.25f);
            _state._moveTimeCur = 0f;

            // 점프는 단일 Impulse로 통합
            Vector3 jumpForce = (_state._movement.normalized + Vector3.up) * jumpPowerCur;
            _rb.AddForce(jumpForce, ForceMode.Impulse);

            Debug.Log($"Jump Power: {jumpPowerCur}");
            
            ResetJumpStatus(); // 250321 추가한 함수
        }

        IEnumerator CrossObstacleHigh()
        {
            _state._isUsingRigidbody = true;
            _rb.AddForce(new Vector3(0f, 2f * _state.jumpPower, 0f), ForceMode.Impulse);
            Debug.Log("2m Obstacle has been set");

            yield return new WaitForSeconds(0.5f);
            _state._isUsingRigidbody = false;
            _rb.AddForce(new Vector3(0f, 0f, 2f * _state.crouchPower), ForceMode.Impulse);
        }

    }


    // Med Jump region
    public void ResetJumpStatus()
    {
        _controller.StartCoroutine(LandCo());
    }
    IEnumerator LandCo()
    {
        _state._jumpAmountCur = _state.jumpAmount;
        _state._isUsingRigidbody = false;
        _state._wallKickStatus = 0;

        if (_state._isRiskyToLand)
        {
            _state._moveSpeedCur = _state.moveSpeedStunned;
            _state.isAccelerating = false;
            Debug.LogWarning("You are too fast to land off without injury");

            yield return new WaitForSeconds(0.75f);
        }
        else
        {
            _state._moveSpeedCur = _state.moveSpeedAfterLand;
            _state.isAccelerating = false;
            //Debug.Log("You have landed");

            yield return new WaitForSeconds(0.3f);
        }
        SetAcceleratingOn();
    }
    public void SetRestrictedLand()
    {
        _rb.linearVelocity = Vector3.zero;
        Debug.LogWarning("You have landed area with restricted jump");
    }

    void SetAcceleratingOn()
    {
        _state._moveSpeedCur = _state.moveSpeed;
        _state._moveTimeCur = 0;
        _state.isAccelerating = true;
    }



}
