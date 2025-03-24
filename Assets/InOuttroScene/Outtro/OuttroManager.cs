using UnityEngine;
using UnityEngine.UI;

public class OuttroManager : MonoBehaviour
{
    static OuttroManager _instance;
    public RobotAnimator robotAnimator;
    public Robot robot;
    public Ocean ocean;
    public Ocean2 ocean2;
    public Block block;
    public static OuttroManager Instance { get { return _instance; } }


    void Awake()
    {
        if (_instance == null)
            _instance = this;

        robot = FindAnyObjectByType<Robot>();
        robotAnimator = robot.GetComponent<RobotAnimator>();
        ocean = FindAnyObjectByType<Ocean>();
        ocean2 = FindAnyObjectByType<Ocean2>();
        block = FindAnyObjectByType<Block>();
    }
}