using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLvl2SFX : MonoBehaviour
{
    AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.GetInstance();
    }
    public void PlayLaserSound()
    {
        audioManager.PlaySFX(audioManager.laserAttack);
    }
    public void PlayLaserChargingSound()
    {
        audioManager.PlaySFX(audioManager.laserCharging);
    }
}
