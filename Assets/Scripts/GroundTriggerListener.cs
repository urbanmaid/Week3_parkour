using UnityEngine;

public class GroundTriggerListener : TriggerListener
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GetComponent<Collider>() == null)
        {
            Debug.LogError("There are no active Collider2D");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
