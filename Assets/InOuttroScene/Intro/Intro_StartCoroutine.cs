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
        IntroManager.Instance.canvas.FadeIn(); //���̵���
        IntroManager.Instance.robotAnimator.SetIdle(); //���̵��
        yield return new WaitForSeconds(3f);
        IntroManager.Instance.robot.MoveForward(1);//ĳ���� �̵�
        yield return new WaitForSeconds(0.5f);
        IntroManager.Instance.robot.RobotChatQMark();// " ? "
        yield return new WaitForSeconds(1.5f);
        IntroManager.Instance.robot.MoveStop();//����
        IntroManager.Instance.ocean.MoveForward(); // �ĵ� ����
        yield return new WaitForSeconds(1f);
        IntroManager.Instance.robot.RobotChatEraze();
        IntroManager.Instance.robot.RobotChatEQMark();
        yield return new WaitForSeconds(1f);
        IntroManager.Instance.robot.TrnasformTurn();
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(0.3f);
        IntroManager.Instance.robot.ParticlePlay();
        IntroManager.Instance.robot.MoveForward(-1); //ĳ���� ����
        yield return new WaitForSeconds(1f);
        IntroManager.Instance.canvas.FadeOut();
    }
}
