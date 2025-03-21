using TMPro;
using UnityEngine;

public class PlayerController_KWS : MonoBehaviour
{
    [Header("Player status")]
    public float moveSpeed = 5f;   // 이동 속도
    public float jumpHeight = 2f;  // 점프 높이
    public float gravity = -9.81f; // 중력 값

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Camera")]
    public Camera followingCamera;
    public Vector3 cameraOffsetPos;
    public Vector3 cameraOffsetRot;
    public float cameraSmoothSpeed = 1f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 땅에 붙도록 보정
        }

        float moveX = Input.GetAxis("Horizontal"); // A(-1) / D(+1)
        float moveZ = Input.GetAxis("Vertical");   // W(+1) / S(-1)

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);


        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }


    //private void LateUpdate()
    //{
    //    Vector3 targetPos = transform.position + cameraOffsetPos;

    //    Vector3 smoothedPosition = Vector3.Lerp(followingCamera.transform.position, targetPos, cameraSmoothSpeed * Time.fixedDeltaTime);
    //    followingCamera.transform.position = smoothedPosition;
    //}
}
