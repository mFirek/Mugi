using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet3 : MonoBehaviour
{
    public float bulletSpeed = 10f;
    private Rigidbody2D rb;
    private Animator animator; // Animator component
    private CircleCollider2D circleCollider; // CircleCollider2D component
    private bool isDestroying = false; // Flag to prevent multiple triggers

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Get the Animator component
        circleCollider = GetComponent<CircleCollider2D>(); // Get the CircleCollider2D component

        // Destroy the bullet after 2 seconds if no collision occurs
        Destroy(gameObject, 0.8f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Teren") || collision.gameObject.CompareTag("Ceiling") || collision.gameObject.CompareTag("Boss") || collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Untagged"))   && !isDestroying)
        {
            isDestroying = true; // Prevent further triggers
            // Play the "Destroy" animation
            animator.SetTrigger("Destroy");

            // Disable the collider right away to prevent further collisions
            circleCollider.enabled = false;

            // Call DestroyObject after animation duration
            Invoke("DestroyObject", 0.5f); // Adjust delay to match the length of the animation
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject); // Actually destroy the bullet after the animation
    }

    // Method to change bullet direction
    void ChangeBulletDirection(Vector2 direction)
    {
        rb.velocity = direction * bulletSpeed;
    }
}
