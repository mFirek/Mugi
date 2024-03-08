using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        
    }

   

    void OnCollisionEnter2D(Collision2D collision)
    {
      
        if (collision.gameObject.CompareTag("Teren"))
        {
            Destroy(gameObject); // Destroy the bullet upon collision with an obstacle
        }
    }

    // Method to change bullet direction
    void ChangeBulletDirection(Vector2 direction)
    {
        rb.velocity = direction * bulletSpeed;
    }
}
