using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource backgroundMusicSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        backgroundMusicSource = GetComponent<AudioSource>();
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        backgroundMusicSource.Play();
    }
    private void StopBackgroundMusic()
    {
        backgroundMusicSource.Stop();
    }

}
