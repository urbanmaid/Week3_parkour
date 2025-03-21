using System.Collections;
using UnityEngine;

public class PlayerActionCrouch
{
    private PlayerController _controller;
    private PlayerState _state;
    private Rigidbody _rb;


    public PlayerActionCrouch(PlayerController controller, PlayerState state, Rigidbody rb)
    {
        _controller = controller;
        _state = state;
        _rb = rb;
    }



    public IEnumerator Crouch()
    {
        if (_state._jumpAmountCur > 0 && _state._moveInput.magnitude > 0.08f && !_state._isCrouching
        && (Mathf.Abs(_state._moveInput.x) > 0.88f || Mathf.Abs(_state._moveInput.y) > 0.88f))
        // Crouch should be done when is on the ground, moving, not crouching
        // and its moving direction is not diagonal
        {
            // Reset Crouch Status
            _state._isUsingRigidbody = true;
            _rb.AddForce(_state.crouchPower * new Vector3(_state._movement.x, _rb.linearVelocity.y / _state.crouchPower, _state._movement.z), ForceMode.Impulse);
            SetColliderCrouch();

            // Reset Crouch Status
            yield return new WaitForSeconds(0.5f);
            _state._isUsingRigidbody = false;
            ResetColliderCrouch();

            //After that makes it able to re-crouch after awhile
            yield return new WaitForSeconds(0.25f);
            _state._isCrouching = false;
        }
    }
    void SetColliderCrouch()
    {
        _state._isCrouching = true;
        _state.capsuleCollider.height = _state._colliderHeightOnCrouch;
        _state.capsuleCollider.center = new Vector3(0f, 0.5f * _state._colliderHeightOnCrouch, 0f);
    }
    void ResetColliderCrouch()
    {
        _state.capsuleCollider.height = _state._colliderHeight;
        _state.capsuleCollider.center = new Vector3(0f, 0.5f * _state._colliderHeight, 0f);
    }

}
