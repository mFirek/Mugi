using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField]  AudioSource musicSource;
    [SerializeField]  AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip bossDefeat;
    public AudioClip Hit;
    public AudioClip laserAttack;
    public AudioClip Jump;
    public AudioClip enemyAttack;
    public AudioClip enemyAttack2;
    public AudioClip Attack;

    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);    
    }
}
