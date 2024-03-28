using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;
  //  public GameObject laserPrefab;
   // public Transform laserSpawnPoint;

    public bool isFlipped = false;

   // public float fireRate = 0.5f; // Okre�l, jak cz�sto boss strzela
 //   private float nextFireTime = 0f;
 
 //   public float laserLifetime = 1f; // Czas �ycia lasera

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

   // void Shoot()
   // {
        // Upewnij si�, �e nie strzelamy, gdy laser jest aktywny
      //  if (activeLaser == null)
      //  {
            // Pobierz wysoko�� punktu spawnu lasera
      //      float laserHeight = laserSpawnPoint.position.y;
            // Skoryguj pozycj� lasera o t� wysoko��
      //      Vector3 laserSpawnPosition = new Vector3(laserSpawnPoint.position.x, laserHeight, laserSpawnPoint.position.z);

            // Wystrzel laser z poprawionej pozycji
       //     activeLaser = Instantiate(laserPrefab, laserSpawnPosition, laserSpawnPoint.rotation, transform);
            // Ustawienie kierunku lasera w stron� gracza (je�li potrzebne)
      //      activeLaser.transform.right = (player.position - laserSpawnPosition).normalized;

            // Zniszcz laser po czasie laserLifetime
        //    Destroy(activeLaser, laserLifetime);
       // }
   // }
}