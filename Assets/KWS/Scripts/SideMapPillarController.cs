using TMPro;
using UnityEngine;

public class SideMapPillarController : MonoBehaviour
{
    public GameObject[] pillars;

    public PlayerController_KWS playerController;

    public int rotateMode = 0;
    public int maxRotateMode = 5;
    public float rotateOffset = 15f;

    public TextMeshProUGUI rotateInfoText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("SideMapPillarController :: RotateEachPillars Called");
            RotateEachPillars();
        }
    }

    private void RotateEachPillars()
    {
        rotateMode += 1;
        rotateMode %= maxRotateMode;

        rotateInfoText.text = $"Rotate Mode: {rotateMode}\nRotateOffset: {rotateOffset}";


        playerController.gameObject.transform.Rotate(rotateOffset, 0, 0);
        for (int i = 0; i < pillars.Length; i++)
        {
            pillars[i].gameObject.transform.Rotate(rotateOffset, 0, 0);
        }

        //playerController.

        if (rotateMode == 0)
        {
            playerController.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            for (int i = 0; i < pillars.Length; i++)
            {
                pillars[i].gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
        }
    }
}
