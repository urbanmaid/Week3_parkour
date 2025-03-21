using UnityEngine;

public class PlayerAnimation
{
    private PlayerController _controller;
    private PlayerState _state;
    private Rigidbody _rb;

    public PlayerAnimation(PlayerController controller, PlayerState state, Rigidbody rb)
    {
        _controller = controller;
        _state = state;
        _rb = rb;
    }


    public void SetAnim()
    {
        if (_state._movement.magnitude < 0.08f)
        {
            if (_state._wallKickStatus == -1)
            {
                _state.robotAnimator.transform.rotation = Quaternion.LookRotation(Vector3.left);
                _state.robotAnimator.SetWallKick();
            }
            else if (_state._wallKickStatus == 1)
            {
                _state.robotAnimator.transform.rotation = Quaternion.LookRotation(Vector3.right);
                _state.robotAnimator.SetWallKick();
            }
            else if (_state._wallKickStatus == -10)
            {
                _state.robotAnimator.transform.rotation = Quaternion.LookRotation(Vector3.right);
                _state.robotAnimator.SetWallKickPrep();
            }
            else if (_state._wallKickStatus == 10)
            {
                _state.robotAnimator.transform.rotation = Quaternion.LookRotation(Vector3.left);
                _state.robotAnimator.SetWallKickPrep();
            }
            else
            {
                if (_state._jumpAmountCur != _state.jumpAmount)
                {
                    _state.robotAnimator.SetJump();
                }
                else
                {
                    if (_state._moveSpeedCur == _state.moveSpeedAfterLand)
                    {
                        _state.robotAnimator.SetJumpLand();
                    }
                    else if (_state._moveSpeedCur == _state.moveSpeedStunned)
                    {
                        _state.robotAnimator.SetDamage();
                    }
                    else
                    {
                        _state.robotAnimator.SetIdle();
                    }
                }
            }
        }
        else
        {
            if (_state._isUsingRigidbody)
            {
                if (_state._isCrouching)
                {
                    _state.robotAnimator.SetCrouch();
                }
            }
            else
            {
                if (_state._jumpAmountCur != _state.jumpAmount)
                {
                    _state.robotAnimator.SetJump();
                }
                else
                {
                    if (_state._moveSpeedCur == _state.moveSpeedAfterLand)
                    {
                        _state.robotAnimator.SetJumpLand();
                    }
                    else if (_state._moveSpeedCur == _state.moveSpeedStunned)
                    {
                        _state.robotAnimator.SetDamage();
                    }
                    else
                    {
                        _state.robotAnimator.SetRun();
                    }
                }

                // Set Avatar rotation
                _state.robotAnimator.transform.rotation = Quaternion.LookRotation(new Vector3(_state._movement.x, 0f, _state._movement.z).normalized);
            }
        }
    }
}
