using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputActions _inputActions;
    private PlayerController _controller;
    private PlayerState _state;

    public void InitInputAction(PlayerController controller, PlayerState state)
    {
        _controller = controller;
        _state = state;
        _inputActions = new InputActions();
        
        _inputActions.Player.Move.performed += ctx => _state._moveInput = ctx.ReadValue<Vector2>();
        _inputActions.Player.Move.canceled += ctx => _state._moveInput = Vector2.zero;
        _inputActions.Player.Jump.performed += ctx => _controller.Jump();
        _inputActions.Player.Crouch.performed += ctx => StartCoroutine(_controller.Crouch());

        _inputActions.Player.Enable();
    }

    void OnEnable()
    {
        if (_inputActions != null)
        {
            _inputActions.Player.Enable(); // Player Action Map Ȱ��ȭ
        }
    }

    void OnDisable()
    {
        if (_inputActions != null)
        {
            _inputActions.Player.Disable(); // ��Ȱ��ȭ �� �Է� ����
        }
    }
}
