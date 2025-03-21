using UnityEngine;
using TMPro;
using System.Collections;
public class Robot : MonoBehaviour
{
    Rigidbody rb;
    public TextMeshPro chat;
    public ParticleSystem particle;

    private Quaternion targetRotation;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRotation = Quaternion.Euler(0, 180, 0);  //회전 목표
        particle.Stop();
    }
    public void MoveForward(int vector)
    {
        if (vector > 0) rb.linearVelocity = Vector3.forward * speed;
        if (vector < 0) rb.linearVelocity = -Vector3.forward * speed;
    }
    public void TrnasformTurn()
    {
        StartCoroutine(RotateSmoothly());
    }
    public void MoveStop()
    {
        rb.linearVelocity = Vector3.zero;
    }
    IEnumerator RotateSmoothly()
    {
        float time = 0;
        Quaternion startRotation = transform.rotation;

        while (time < 1)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time);
            time += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        transform.rotation = targetRotation;  // 정확한 최종 값 설정
    }

    public void ParticlePlay()
    {
        particle.Play();
    }
    public void RobotChatQMark()
    {
        chat.text = " ? ";
    }
    public void RobotChatEQMark()
    {
        chat.text = " ! ";
    }
    public void RobotChatEraze()
    {
        chat.text = "  ";
    }
}

