using UnityEngine;

public class PlayerAchievement
{
    private PlayerController _controller;
    private PlayerState _state;
    private Rigidbody _rb;

    public PlayerAchievement(PlayerController controller, PlayerState state, Rigidbody rb)
    {
        _controller = controller;
        _state = state;
        _rb = rb;
    }


    public void SetCollisionLow()
    {
        if (_state.capsuleCollider.height == _state._colliderHeight)
        {
            AchievementManager.instance.UpdateCollisionLow();
        }
    }
    public void SetCollisionHigh()
    {
        if (_state.capsuleCollider.height == _state._colliderHeight)
        {
            if ((_rb.linearVelocity.y > -0.02f || 0.02f > _rb.linearVelocity.y)
            && _state.triggerFeet.isTriggered) // If player is on the ground and velocity axis y is nearly 0
            {
                AchievementManager.instance.UpdateCollisionHigh();
            }
        }
    }

    public void SetAccumulatedDist()
    {
        AchievementManager.instance.UpdateDist(Mathf.Round(_rb.linearVelocity.magnitude * Time.deltaTime * 100f) / 100f);
    }
}
