using UnityEngine;

public class GroundTriggerListener : TriggerListener
{
    // Includes TriggerListener Variables
    [SerializeField] int triggerCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GetComponent<Collider>() == null)
        {
            Debug.LogError("There are no active Collider");
        }
    }

    public override void OnTriggerEnter(Collider collision)
    {
        // 대상 태그 확인 로직
        bool shouldTrigger = CheckTriggerCondition(collision);

        if (shouldTrigger)
        {
            triggerCount++; // 겹친 트리거 수 증가
            if (!isTriggered) // 처음 활성화 시에만 상태 변경
            {
                isTriggered = true;
                actionWhenTriggered.Invoke();
            }
        }
    }

    public override void OnTriggerExit(Collider collision)
    {
        // 대상 태그 확인 로직
        bool wasTriggered = CheckTriggerCondition(collision);

        if (wasTriggered)
        {
            triggerCount--; // 겹친 트리거 수 감소
            triggerCount = Mathf.Max(0, triggerCount); // 음수 방지

            if (triggerCount == 0) // 모든 트리거가 사라졌을 때만 비활성화
            {
                isTriggered = false;
            }
        }
    }

    bool CheckTriggerCondition(Collider collision)
    {
        if (targetOfTrigger != "") // 태그 필터링 사용 시
        {
            return collision.CompareTag(targetOfTrigger);
        }
        return true; // 태그 필터링 없으면 항상 true
    }
}
