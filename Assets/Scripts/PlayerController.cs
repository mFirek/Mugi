using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public LayerMask groundMask;
    public float bulletSpeed = 1f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpsRemaining;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsRemaining = 2; // Ustawia pocz¹tkow¹ liczbê skoków
    }

    void Update()
    {
        // SprawdŸ, czy gracz jest na ziemi
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, groundMask);

        // Ruch w poziomie
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Skakanie
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && jumpsRemaining > 0)
        {
            if (!isGrounded) // Jeœli gracz nie jest na ziemi, zmniejsz liczbê pozosta³ych skoków
            {
                jumpsRemaining--;
            }
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Strzelanie
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = firePoint.right * bulletSpeed; // Zak³adaj¹c, ¿e pocisk porusza siê w kierunku, w którym patrzy gracz
    }

    // Zresetuj liczbê skoków, gdy gracz dotknie ziemi
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Teren"))
        {
            jumpsRemaining = 2;
        }
    }
}