using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] PlayerController playerController;
    [SerializeField] ChunkManager chunkManager;

    [SerializeField] private float[] resetPos = { 5f, 20f, 35f};
    private Vector3 resetRelativePos = new(0, 1.5f, -6f);

    private int curStage = 1;

    //public EnvironmentOffset environmentOffset;


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
        //playerController.SetCollided();
        playerController.StopPlayer();
    }

    public void ResetPlayerInRelatedPos(Vector3 resetPos)
    {
        playerController.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        playerController.gameObject.transform.position = resetPos + resetRelativePos;
        playerController.SetCollidedStatus(false);
    }

    public void ResetPlayerPosition()
    {
        Vector3 curPos = playerController.gameObject.transform.position;

        float curZ = curPos.z;
        int chunkIndex = Mathf.FloorToInt(curZ / 50);
        float chunkStartZ = chunkIndex * 50;

        Vector3 resetPos = new Vector3(0f, playerController.fallOffset + 10f, 0f);

        float relativeZ = curZ - chunkStartZ;

        float targetZOffset = resetPos[0];
        if (relativeZ > resetPos[1])
        {
            targetZOffset = resetPos[1];
        }
        else if (relativeZ > resetPos[2])
        {
            targetZOffset = resetPos[2];
        }
        else
        {
            targetZOffset = resetPos[0];
        }

        resetPos.z = chunkStartZ + targetZOffset;

        playerController.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        playerController.gameObject.transform.position = resetPos;

        Debug.Log("Player Position Reset");
    }

    public void SetCurStage(int stage)
    {
        curStage = stage;
        gameObject.GetComponent<EnvironmentOffset>().SetStageTheme(curStage);
        playerController.fallOffset = gameObject.GetComponent<ChunkManager>().GetStageHeight();
    }



    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
