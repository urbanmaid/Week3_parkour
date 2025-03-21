using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    //[SerializeField] int countCollisionLow = 0;
    //[SerializeField] int countCollisionHigh = 0;
    [SerializeField] float countDist = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    /*
    public void UpdateCollisionLow()
    {
        countCollisionLow++;
    }
    public void UpdateCollisionHigh()
    {
        countCollisionHigh++;
    }
    */
    public void UpdateDist(float distance)
    {
        countDist += distance;
    }
}
