using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet3 : MonoBehaviour
{
    public float bulletSpeed = 10f;
    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Destroy the bullet after 2 seconds
        Destroy(gameObject, 2f);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
      
        if (collision.gameObject.CompareTag("Teren") | collision.gameObject.CompareTag("Ceiling"))
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
