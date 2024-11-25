using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private float musicVolume;
    private float sfxVolume;

    private void Start()
    {
        // Ustaw domyœlne wartoœci, jeœli brak zapisanych ustawieñ
        if (!PlayerPrefs.HasKey("musicVolume") || !PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.5f); // Domyœlna wartoœæ 50% g³oœnoœci
            PlayerPrefs.SetFloat("sfxVolume", 0.5f);   // Domyœlna wartoœæ 50% g³oœnoœci
            PlayerPrefs.Save();
        }

        // Za³aduj zapisane ustawienia
        Load();

        // Zastosuj ustawienia g³oœnoœci
        ApplyVolumeSettings();

        // Upewnij siê, ¿e dŸwiêki s¹ odtwarzane
        EnsureSoundsArePlaying();
    }

    public void ChangeMusicVolume()
    {
        musicVolume = musicVolumeSlider.value;
        SaveMusicVolume();
        ApplyMusicVolume();
    }

    public void ChangeSFXVolume()
    {
        sfxVolume = sfxVolumeSlider.value;
        SaveSFXVolume();
        ApplySFXVolume();
    }

    private void Load()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume");

        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;
    }

    private void SaveMusicVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    private void SaveSFXVolume()
    {
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    private void ApplyVolumeSettings()
    {
        ApplyMusicVolume();
        ApplySFXVolume();
    }

    private void ApplyMusicVolume()
    {
        AudioManager audioManager = AudioManager.GetInstance();

        if (audioManager != null && audioManager.musicSource != null)
        {
            audioManager.musicSource.volume = musicVolume; // Ustaw g³oœnoœæ muzyki
        }
        else
        {
            Debug.LogWarning("AudioManager or musicSource not found.");
        }
    }

    private void ApplySFXVolume()
    {
        AudioManager audioManager = AudioManager.GetInstance();

        if (audioManager != null && audioManager.sfxSource != null)
        {
            audioManager.sfxSource.volume = sfxVolume; // Ustaw g³oœnoœæ efektów dŸwiêkowych
        }
        else
        {
            Debug.LogWarning("AudioManager or sfxSource not found.");
        }
    }

    private void EnsureSoundsArePlaying()
    {
        AudioManager audioManager = AudioManager.GetInstance();

        if (audioManager != null)
        {
            if (audioManager.musicSource != null && !audioManager.musicSource.isPlaying)
            {
                audioManager.musicSource.Play(); // Upewnij siê, ¿e muzyka gra
            }

            if (audioManager.sfxSource != null && audioManager.sfxSource.clip != null && !audioManager.sfxSource.isPlaying)
            {
                audioManager.sfxSource.PlayOneShot(audioManager.sfxSource.clip); // Odtwórz przyk³adowy dŸwiêk
            }
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found.");
        }
    }
}
