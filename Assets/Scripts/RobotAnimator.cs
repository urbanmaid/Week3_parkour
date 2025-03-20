using System;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimator : MonoBehaviour
{
    [SerializeField] List<GameObject> objectAnimated;

    [Header("Rotation definer")]
    [SerializeField] List<Vector3> rotationIdle;
    [SerializeField] List<Vector3> rotationRun;
    [SerializeField] List<Vector3> rotationJump;
    [SerializeField] List<Vector3> rotationJumpLand;
    [SerializeField] List<Vector3> rotationWallKick;
    [SerializeField] List<Vector3> rotationCrouch;
    [SerializeField] List<Vector3> rotationDamage;
    [SerializeField] List<Vector3> rotationDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckIntegrity();
    }

    private void CheckIntegrity()
    {
        if(objectAnimated.Count != rotationIdle.Count
        || objectAnimated.Count != rotationRun.Count
        /*
        || objectAnimated.Count != rotationJump.Count
        || objectAnimated.Count != rotationJumpLand.Count
        || objectAnimated.Count != rotationWallKick.Count
        || objectAnimated.Count != rotationCrouch.Count
        || objectAnimated.Count != rotationDamage.Count
        || objectAnimated.Count != rotationDeath.Count
        */
        )
        {
            Debug.LogError("There are some discord around the count with object and rotation Vector3");
        }
    }

    // Update is called once per frame
    internal void SetIdle()
    {
        for(int i = 0; i < objectAnimated.Count; i++)
        {
            objectAnimated[i].transform.localRotation = Quaternion.Euler(rotationIdle[i]);
        }
    }
    internal void SetRun()
    {
        for(int i = 0; i < objectAnimated.Count; i++)
        {
            objectAnimated[i].transform.localRotation = Quaternion.Euler(rotationRun[i]);
        }
    }
    internal void SetJump()
    {
        for(int i = 0; i < objectAnimated.Count; i++)
        {
            objectAnimated[i].transform.localRotation = Quaternion.Euler(rotationJump[i]);
        }
    }

    internal void SetWallKick()
    {
        for(int i = 0; i < objectAnimated.Count; i++)
        {
            objectAnimated[i].transform.localRotation = Quaternion.Euler(rotationWallKick[i]);
        }
    }
}
