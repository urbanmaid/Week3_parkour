using UnityEngine;
using UnityEngine.Events;

public class TriggerListener : MonoBehaviour
{
    public bool isTriggered = false;
    public string targetOfTrigger = "";
    public UnityEvent actionWhenTriggered;

    void Start()
    {
        if(GetComponent<Collider>() == null)
        {
            Debug.LogError("There are no active Collider2D");
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if(targetOfTrigger != "") //If this element uses targetOfTrigger
        {
            if(collision.CompareTag(targetOfTrigger))
            {
                isTriggered = true;
                actionWhenTriggered.Invoke();
            }
        }
        else
        {
            isTriggered = true;
            actionWhenTriggered.Invoke();
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if(targetOfTrigger != "") //If this element uses targetOfTrigger
        {
            if(collision.CompareTag(targetOfTrigger))
            {
                isTriggered = false;
            }
        }
        else
        {
            isTriggered = false;
        }
    }
}
