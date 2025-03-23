using System.Collections;
using UnityEngine;

public class PitfallFX : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particleObjects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StopParticles();
    }

    // Update is called once per frame
    public void ResetPlayer()
    {
        StartCoroutine(ResetPlayerCo());
    }

    IEnumerator ResetPlayerCo()
    {
        GameManager.instance.SetPlayerVisible(false);
        GameManager.instance.SetPlayerCollided();
        PlayParticles();

        yield return new WaitForSeconds(1.5f);
        GameManager.instance.SetPlayerVisible(true);
        GameManager.instance.ResetPlayerInRelatedPos(transform.position);
        StopParticles();
    }

    void PlayParticles()
    {
        for(int i = 0; i < particleObjects.Length; i++)
        {
            particleObjects[i].Play();
        }
    }

    void StopParticles()
    {
        for(int i = 0; i < particleObjects.Length; i++)
        {
            particleObjects[i].Stop();
        }
    }
}
