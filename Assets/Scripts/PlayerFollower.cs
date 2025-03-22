using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    // Update is called once per frame
    public void DoUpdate()
    {
        transform.position = new(
            0,
            playerController.transform.position.y,
            playerController.transform.position.z
        );
    }
}
