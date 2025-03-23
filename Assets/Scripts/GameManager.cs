using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] PlayerController playerController;
    [SerializeField] ChunkManager chunkManager;

    [SerializeField] private float[] resetPos = { 5f, 20f, 35f};
    private Vector3 resetRelativePos = new(0, 1.5f, -6f);
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        playerController.DoUpdate();
    }

    public void SetPlayerVisible(bool value)
    {
        playerController.robotAnimator.gameObject.SetActive(value);
        playerController.SetUsingRigidbody(!value);
    }

    public void SetPlayerCollided()
    {
        playerController.SetCollided();
    }

    public void ResetPlayerInRelatedPos(Vector3 resetPos)
    {
        playerController.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        playerController.gameObject.transform.position = resetPos + resetRelativePos;
    }

    public void ResetPlayerPosition()
    {
        Vector3 curPos = playerController.gameObject.transform.position;
        float curZ = curPos.z;

        int chunkIndex = Mathf.FloorToInt(curZ / 50);
        float chunkStartZ = chunkIndex * 50;
        float relativeZ = curZ - chunkStartZ;

        Vector3 resetPos = new Vector3(0f, 25f, chunkStartZ); // 기본 위치
        if (relativeZ > 25f) // 청크 중간 이상이면 오프셋 추가
        {
            resetPos.z += 25f;
        }

        playerController.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        playerController.gameObject.transform.position = resetPos;

        Debug.Log("Player Position Reset");
    }
}
