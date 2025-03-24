using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{


    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().buildIndex == 1) // Tutorial Scene
            {
                GameManager.instance.LoadNextScene();
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2) // Stage Scene
            {
                if (GameManager.instance.GetCurStage() == 4)
                {
                    GameManager.instance.LoadNextScene();
                }
            }
        }
    }
    */

    public void GetNetxScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) // Tutorial Scene
        {
            GameManager.instance.LoadNextScene();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2) // Stage Scene
        {
            Debug.Log(GameManager.instance.GetCurStage() + ": Curstage");
            if (GameManager.instance.chunkManager.isGameFinished)
            {
                GameManager.instance.LoadNextScene();
            }
        }
    }
}
