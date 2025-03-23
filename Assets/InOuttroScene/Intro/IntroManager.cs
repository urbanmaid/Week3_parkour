using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    static IntroManager _instance;
    public RobotAnimator robotAnimator;
    public Canvas canvas;
    public Robot robot;
    public Ocean ocean;
    public static IntroManager Instance { get { return _instance; } }


    void Awake()
    {
        if (_instance == null)
            _instance = this;

        robotAnimator = FindAnyObjectByType<RobotAnimator>();
        robot = FindAnyObjectByType<Robot>();
        canvas = FindAnyObjectByType<Canvas>();
        ocean = FindAnyObjectByType<Ocean>();
    }
}