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

        // Zastosuj ustawienia i rozpocznij odtwarzanie dŸwiêków
        ApplyVolumeSettings();
        PlaySounds();
    }

    public void ChangeMusicVolume()
    {
        musicVolume = musicVolumeSlider.value;
        Save();
        ApplyVolumeSettings();
    }

    public void ChangeSFXVolume()
    {
        sfxVolume = sfxVolumeSlider.value;
        Save();
        ApplyVolumeSettings();
    }

    private void Load()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume");

        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    private void ApplyVolumeSettings()
    {
        AudioManager audioManager = AudioManager.GetInstance();

        if (audioManager != null)
        {
            audioManager.musicSource.volume = musicVolume; // Ustaw g³oœnoœæ muzyki
            audioManager.sfxSource.volume = sfxVolume;     // Ustaw g³oœnoœæ efektów dŸwiêkowych
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found.");
        }
    }

    private void PlaySounds()
    {
        AudioManager audioManager = AudioManager.GetInstance();

        if (audioManager != null)
        {
            if (!audioManager.musicSource.isPlaying) // Jeœli muzyka nie gra, uruchom
            {
                audioManager.musicSource.Play();
            }

            if (!audioManager.sfxSource.isPlaying) // Jeœli efekty dŸwiêkowe nie graj¹, odtwórz przyk³adowy dŸwiêk
            {
                audioManager.sfxSource.PlayOneShot(audioManager.sfxSource.clip);
            }
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found.");
        }
    }
}
