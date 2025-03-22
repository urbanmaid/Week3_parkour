using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] PlayerController playerController;


    [SerializeField] private float[] resetPos = { 5f, 20f, 35f};
    
    
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


    public void ResetPlayerPosition()
    {
        Vector3 curPos = playerController.gameObject.transform.position;

        float curZ = curPos.z;
        int chunkIndex = Mathf.FloorToInt(curZ / 50);
        float chunkStartZ = chunkIndex * 50;

        Vector3 resetPos = new Vector3(0f, 10f, 0f);

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
}
