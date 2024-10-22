using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;
  //  public GameObject laserPrefab;
   // public Transform laserSpawnPoint;

    public bool isFlipped = false;

    AudioManager audioManager;

    // public float fireRate = 0.5f; // Okreœl, jak czêsto boss strzela
    //   private float nextFireTime = 0f;

    //   public float laserLifetime = 1f; // Czas ¿ycia lasera

    // private GameObject activeLaser; // Referencja do aktywnego lasera

    //  void Update()
    //{
    //     LookAtPlayer();
    //    if (Time.time >= nextFireTime)
    //    {
    //         Shoot();
    //   nextFireTime = Time.time + 1f / fireRate;
    //    }
    // }
    public float specialAttackCooldown = 5f;
    public float shieldCooldown = 4f;

    [HideInInspector]
    public float lastSpecialAttackTime = -Mathf.Infinity;
    [HideInInspector]
    public float lastShieldTime = -Mathf.Infinity;

    private void Start()
    {
        audioManager = AudioManager.GetInstance();
    }

    public void PlayAttackSound()
    {
        audioManager.PlaySFX(audioManager.Fmelee);
    }

    public void PlayAttackPhase2()
    {
        audioManager.PlaySFX(audioManager.SpellPhase2);
    }

    public void PlayTransform()
    {
        audioManager.PlaySFX(audioManager.PowerUp);
    }

    public void PlaySummonSound()
    {
        audioManager.PlaySFX(audioManager.Summon);
    }
    public void PlayShieldSound()
    {
        audioManager.PlaySFX(audioManager.FairyShield);
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
  
    public void SpecialAttack()
    {
        // Implementacja specjalnego ataku
        Debug.Log("Boss wykonuje specjalny atak!");
    }

    public void NormalAttack()
    {
        // Implementacja normalnego ataku
        Debug.Log("Boss wykonuje normalny atak!");
    }

    public void UseShield()
    {
        // Implementacja u¿ycia tarczy
        Debug.Log("Boss u¿ywa tarczy!");
    }
    // void Shoot()
    // {
    // Upewnij siê, ¿e nie strzelamy, gdy laser jest aktywny
    //  if (activeLaser == null)
    //  {
    // Pobierz wysokoœæ punktu spawnu lasera
    //      float laserHeight = laserSpawnPoint.position.y;
    // Skoryguj pozycjê lasera o tê wysokoœæ
    //      Vector3 laserSpawnPosition = new Vector3(laserSpawnPoint.position.x, laserHeight, laserSpawnPoint.position.z);

    // Wystrzel laser z poprawionej pozycji
    //     activeLaser = Instantiate(laserPrefab, laserSpawnPosition, laserSpawnPoint.rotation, transform);
    // Ustawienie kierunku lasera w stronê gracza (jeœli potrzebne)
    //      activeLaser.transform.right = (player.position - laserSpawnPosition).normalized;

    // Zniszcz laser po czasie laserLifetime
    //    Destroy(activeLaser, laserLifetime);
    // }
    // }
}
