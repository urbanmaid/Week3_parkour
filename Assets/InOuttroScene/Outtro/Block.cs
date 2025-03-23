using UnityEngine;

public class Block : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void MoveUp()
    {
        rb.linearVelocity = 1.2f * Vector3.up;
    }
    public void MoveStop()
    {
        rb.linearVelocity = Vector3.zero;
    }
}
