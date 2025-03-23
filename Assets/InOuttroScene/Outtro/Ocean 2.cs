using UnityEngine;

public class Ocean2 : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    float _moveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void MoveForward()
    {
        rb.linearVelocity = _moveSpeed * Vector3.forward;
    }    public void MoveStop()
    {
        rb.linearVelocity = Vector3.zero;
    }
}
