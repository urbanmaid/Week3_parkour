using UnityEngine;

public class Ocean : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void MoveForward()
    {
        rb.linearVelocity = -Vector3.forward;
    }
}
