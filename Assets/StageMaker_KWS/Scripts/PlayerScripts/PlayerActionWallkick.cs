using UnityEngine;

public class PlayerActionWallkick
{
    private PlayerController _controller;
    private PlayerState _state;
    private Rigidbody _rb;

    public PlayerActionWallkick(PlayerController controller, PlayerState state, Rigidbody rb)
    {
        _controller = controller;
        _state = state;
        _rb = rb;
    }



    public void WallKickL()
    {
        Debug.Log("Doing Wallkick");
        _state._isUsingRigidbody = true;
        _state._wallKickStatus = -1;
        _rb.AddForce(_state.jumpPower * Vector3.Scale(_state._wallKickDirection, new Vector3(-1f, 1f, 1f)), ForceMode.Impulse);
    }
    public void WallKickR()
    {
        _state._isUsingRigidbody = true;
        _state._wallKickStatus = 1;
        _rb.AddForce(_state.jumpPower * _state._wallKickDirection, ForceMode.Impulse);
    }
    public void SetWallKickPrep(int value)
    {
        if (_state._jumpAmountCur != _state.jumpAmount) //!triggerFeet.isTriggered
        {
            Debug.Log("You need to be on air to set the prep mode of anim.");
            _state._wallKickStatus = value;
        }
    }
}
