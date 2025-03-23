using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Canvas : MonoBehaviour
{
    private float fadeDuration = 1.5f;  // 페이드 지속 시간
    public Image image;


    void Start()
    {
        SetClosedState();
    }

    // 커튼 열기 (페이드 아웃: 불투명 → 투명)
    public IEnumerator OpenCurtain()
    {
        float time = 0;
        Color color = image.color;

        while (time < fadeDuration)
        {
            color.a = Mathf.Lerp(1, 0, time / fadeDuration);  // 알파 값 1 → 0
            image.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        // 완전히 열린 상태 유지
        SetOpenState();
    }

    // 커튼 닫기 (페이드 인: 투명 → 불투명)
    public IEnumerator CloseCurtain()
    {
        float time = 0;
        Color color = image.color;

        while (time < fadeDuration)
        {
            color.a = Mathf.Lerp(0, 1, time / fadeDuration);  // 알파 값 0 → 1
            image.color = color;
            time += Time.deltaTime;
            yield return null;
        }

        // 완전히 닫힌 상태 유지
        SetClosedState();
    }

    // 열린 상태 유지 (투명)
    private void SetOpenState()
    {
        Color color = image.color;
        color.a = 0;  // 완전 투명
        image.color = color;
    }

    // 닫힌 상태 유지 (불투명)
    private void SetClosedState()
    {
        Color color = image.color;
        color.a = 1;  // 완전 불투명
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
