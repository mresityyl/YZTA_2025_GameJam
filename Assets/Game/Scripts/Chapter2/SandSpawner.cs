using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandSpawner : MonoBehaviour
{
    public GameObject sandPrefab;
    public float spawnInterval = 1f;
    public float throwForce = 5f;
    public float angleRange = 15f;
    public int maxFallingSand = 15;

    private List<GameObject> activeSands = new List<GameObject>();

    public bool isTimeFreeze = false;

    public AudioSource clockSound;
    public AudioSource music;

    public float targetPitch = 1f;
    public float pitchLerpSpeed = 2f;

    void Start()
    {
        StartCoroutine(SpawnSandRoutine());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isTimeFreeze)
            {
                FreezeAllSands();
                StopAllCoroutines();

                isTimeFreeze = true;

                clockSound.Pause();
                targetPitch = 0.3f;
            }
            else
            {
                isTimeFreeze = false;

                StartCoroutine(SpawnSandRoutine());

                clockSound.Play();
                targetPitch = 1f;
            }
        }
        music.pitch = Mathf.Lerp(music.pitch, targetPitch, Time.deltaTime * pitchLerpSpeed);

    }

    IEnumerator SpawnSandRoutine()
    {
        while (true)
        {
            // Sadece 15 aktif obje varsa yeni bir tane spawn et
            if (activeSands.Count < maxFallingSand)
            {
                GameObject sand = Instantiate(sandPrefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = sand.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    float randomAngle = Random.Range(-angleRange, angleRange);
                    Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * Vector2.down;
                    rb.linearVelocity = direction.normalized * throwForce;
                }

                activeSands.Add(sand);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void FreezeAllSands()
    {
        foreach (GameObject sand in activeSands)
        {
            if (sand != null)
            {
                Rigidbody2D rb = sand.GetComponent<Rigidbody2D>();
                CircleCollider2D circleCollider = sand.GetComponent<CircleCollider2D>();
                BoxCollider2D boxCollider = sand.GetComponent<BoxCollider2D>();
                if (rb != null)
                {
                    circleCollider.enabled = false;
                    boxCollider.enabled = true;

                    rb.linearVelocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }
            }
        }

        activeSands.Clear();
    }
}
