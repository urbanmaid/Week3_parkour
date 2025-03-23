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

        robotAnimator = FindAnyObjectByType<RobotAnimator>();
        robot = FindAnyObjectByType<Robot>();
        ocean = FindAnyObjectByType<Ocean>();
        ocean2 = FindAnyObjectByType<Ocean2>();
        block = FindAnyObjectByType<Block>();
    }
}