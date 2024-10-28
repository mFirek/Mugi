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
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            PlayerPrefs.SetFloat("sfxVolume", 1);
            Load();
        }
        else
        {
            Load();
        }

        ApplyVolumeSettings();
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
}
