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
        OuttroManager.Instance.robot.MoveForward(-1);//ĳ���� �̵�
        OuttroManager.Instance.ocean.MoveForward();//�ٴ�1 �̵�
        OuttroManager.Instance.robotAnimator.SetIdle(); //���̵��
        yield return new WaitForSeconds(2f);
        OuttroManager.Instance.block.MoveUp();
        yield return new WaitForSeconds(1f);
        OuttroManager.Instance.robot.MoveStop();//ĳ���� ����
        OuttroManager.Instance.robot.TrnasformTurn();
        OuttroManager.Instance.robot.RobotChatQMark();//
        OuttroManager.Instance.ocean.MoveStop();//�ٴ�1 ����
        OuttroManager.Instance.ocean2.MoveForward();//�ٴ�2 �̵�
        yield return new WaitForSeconds(2.5f);
        OuttroManager.Instance.block.MoveStop();//�� ����
        yield return new WaitForSeconds(0.7f);
        OuttroManager.Instance.ocean2.MoveStop();//�ٴ�1 ����
        yield return new WaitForSeconds(0.5f);
        OuttroManager.Instance.robot.RobotChatEraze();

    }
}
