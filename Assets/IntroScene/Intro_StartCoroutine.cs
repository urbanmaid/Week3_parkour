using System.Collections;
using UnityEngine;

public class Intro_StartCoroutine : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(IntroStart());
    }
    IEnumerator IntroStart()
    {        
        yield return new WaitForSeconds(0.5f);
        IntroManager.Instance.canvas.FadeIn(); //페이드인
        IntroManager.Instance.robotAnimator.SetIdle(); //아이들셋
        yield return new WaitForSeconds(3f);
        IntroManager.Instance.robot.MoveForward(1);//캐릭터 이동
        yield return new WaitForSeconds(0.5f);
        IntroManager.Instance.robot.RobotChatQMark();// " ? "
        yield return new WaitForSeconds(1.5f);
        IntroManager.Instance.robot.MoveStop();//정지
        IntroManager.Instance.ocean.MoveForward(); // 파도 접근
        yield return new WaitForSeconds(1f);
        IntroManager.Instance.robot.RobotChatEraze();
        IntroManager.Instance.robot.RobotChatEQMark();
        yield return new WaitForSeconds(1f);
        IntroManager.Instance.robot.TrnasformTurn();
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(0.3f);
        IntroManager.Instance.robot.ParticlePlay();
        IntroManager.Instance.robot.MoveForward(-1); //캐릭터 도망
        yield return new WaitForSeconds(1f);
        IntroManager.Instance.canvas.FadeOut();
    }
}
