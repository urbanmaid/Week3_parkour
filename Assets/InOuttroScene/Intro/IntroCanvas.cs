using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Canvas : MonoBehaviour
{
    private float fadeDuration = 1.5f;  // ���̵� ���� �ð�
    public Image image;


    void Start()
    {
        SetClosedState();
    }

    // Ŀư ���� (���̵� �ƿ�: ������ �� ����)
    public IEnumerator OpenCurtain()
    {
        float time = 0;
        Color color = image.color;

        while (time < fadeDuration)
        {
            color.a = Mathf.Lerp(1, 0, time / fadeDuration);  // ���� �� 1 �� 0
            image.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        // ������ ���� ���� ����
        SetOpenState();
    }

    // Ŀư �ݱ� (���̵� ��: ���� �� ������)
    public IEnumerator CloseCurtain()
    {
        float time = 0;
        Color color = image.color;

        while (time < fadeDuration)
        {
            color.a = Mathf.Lerp(0, 1, time / fadeDuration);  // ���� �� 0 �� 1
            image.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        // ������ ���� ���� ����
        SetClosedState();
    }

    // ���� ���� ���� (����)
    private void SetOpenState()
    {
        Color color = image.color;
        color.a = 0;  // ���� ����
        image.color = color;
    }

    // ���� ���� ���� (������)
    private void SetClosedState()
    {
        Color color = image.color;
        color.a = 1;  // ���� ������
        image.color = color;
    }
    public void FadeIn()
    {
        StartCoroutine(OpenCurtain());
    }

    public void FadeOut()
    {
        StartCoroutine(CloseCurtain());
        GameManager.instance.LoadNextScene();
    }
}
