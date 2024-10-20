using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip bossDefeat;
    public AudioClip Hit;
    public AudioClip laserAttack;
    public AudioClip Jump;
    public AudioClip enemyAttack;
    public AudioClip enemyAttack2;
    public AudioClip Attack;
    public AudioClip Punch;

    private void Awake()
    {
        // Implementacja singletona
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Zniszcz dodatkowe instancje AudioManager
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Utrzymanie AudioManager miêdzy scenami
        }
    }

    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public static AudioManager GetInstance()
    {
        return instance;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
