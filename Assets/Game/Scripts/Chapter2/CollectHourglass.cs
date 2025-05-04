using System.Collections;
using UnityEngine;

public class CollectHourglass : MonoBehaviour
{
    public ParticleSystem collectEffect;
    public AudioSource collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(WaitLoadAndNextScene(collectEffect.main.duration));
            collectEffect?.Play();
            collectSound?.Play();
        }
    }
    IEnumerator WaitLoadAndNextScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Scenecontroller.Instance.LoadNextScene("Chapter2End");
    }

}
