using UnityEngine;

public class Ocean : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    float _moveSpeed;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void MoveForward()
    {
        rb.linearVelocity = -_moveSpeed * Vector3.forward;
    }
    public void MoveStop()
    {
        rb.linearVelocity = Vector3.zero;
    }
}
