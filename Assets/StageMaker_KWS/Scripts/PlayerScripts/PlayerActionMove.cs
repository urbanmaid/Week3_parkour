using UnityEngine;

public class PlayerActionMove
{
    private PlayerController _controller;
    private PlayerState _state;
    private Rigidbody _rb;


    public PlayerActionMove(PlayerController controller, PlayerState state, Rigidbody rb)
    {
        _controller = controller;
        _state = state;
        _rb = rb;
    }


    public void Move()
    {
        // Only Z Axis(Front-rear can be applied its accelation)
        _state._movement = Vector3.Scale(new Vector3(_state._moveInput.x, 0f, _state._moveInput.y).normalized, new Vector3(_state.moveSpeed, 1f, _state._moveSpeedCur));
        if (!_state._isUsingRigidbody)
        {
            // If player tend to stay in wall, make it unable
            if ((_state.triggerShoulderL.isTriggered && _state._moveInput.x > 0f)
             || (_state.triggerShoulderR.isTriggered && _state._moveInput.x < 0f))
            {
                _state._movement.x = 0f;
            }
            if (_state.triggerToe.isTriggered && _state._moveInput.y > 0f)
            {
                _state._movement.z = 0f;
            }

            // Move with linearVelocity
            _rb.linearVelocity = new Vector3(_state._movement.x, _rb.linearVelocity.y, _state._movement.z); // Y축은 점프에만 영향
        }

        // Check move duration
        if (_state._movement.magnitude < 0.08f)
        {
            _state._moveTimeCur = 0;
        }
        else
        {
            _controller.SetAccumulatedDist();
            if (_state._moveTimeMax > _state._moveTimeCur)
            {
                _state._moveTimeCur += Time.deltaTime;
            }
        }
    }

    public void SetMoveSpeed()
    {
        if (_state.isAccelerating)
        {
            _state._moveSpeedCur = _state.moveSpeed + ((_state.moveSpeedMax - _state.moveSpeed) * (_state._moveTimeCur / _state._moveTimeMax));
        }
    }
}
