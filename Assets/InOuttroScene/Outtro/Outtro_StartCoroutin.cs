using System.Collections;
using UnityEngine;

public class Outtro_StartCoroutine : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(OuttroStart());
    }
    IEnumerator OuttroStart()
    {
        OuttroManager.Instance.robot.MoveForward(-1);//캐릭터 이동
        OuttroManager.Instance.ocean.MoveForward();//바다1 이동
        OuttroManager.Instance.robotAnimator.SetIdle(); //아이들셋
        yield return new WaitForSeconds(2f);
        OuttroManager.Instance.block.MoveUp();
        yield return new WaitForSeconds(1f);
        OuttroManager.Instance.robot.MoveStop();//캐릭터 정지
        OuttroManager.Instance.robot.TrnasformTurn();
        OuttroManager.Instance.robot.RobotChatQMark();//
        OuttroManager.Instance.ocean.MoveStop();//바다1 정지
        OuttroManager.Instance.ocean2.MoveForward();//바다2 이동
        yield return new WaitForSeconds(2.5f);
        OuttroManager.Instance.block.MoveStop();//블럭 정지
        yield return new WaitForSeconds(0.7f);
        OuttroManager.Instance.ocean2.MoveStop();//바다1 정지
        yield return new WaitForSeconds(0.5f);
        OuttroManager.Instance.robot.RobotChatEraze();

    }
}
