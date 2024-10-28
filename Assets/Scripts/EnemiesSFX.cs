using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSFX : MonoBehaviour
{

    AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.GetInstance();
    }
    public void PlayEnemySFX()
    {
        audioManager.PlaySFX(audioManager.enemyAttack);
    }
    public void PlayEnemySFX2()
    {
        audioManager.PlaySFX(audioManager.enemyAttack2);
    }
}
