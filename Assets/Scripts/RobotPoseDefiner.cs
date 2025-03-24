using UnityEngine;

public class RobotPoseDefiner : MonoBehaviour
{
    RobotAnimator robotAnimator;
    void Start()
    {
        robotAnimator = GetComponent<RobotAnimator>();
        robotAnimator.SetIdle();
    }
}
