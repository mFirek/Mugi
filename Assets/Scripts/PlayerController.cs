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
        jumpsRemaining = 2; // Ustawia pocz�tkow� liczb� skok�w
    }

    void Update()
    {
        // Sprawd�, czy gracz jest na ziemi
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, groundMask);

        // Ruch w poziomie
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Skakanie
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && jumpsRemaining > 0)
        {
            if (!isGrounded) // Je�li gracz nie jest na ziemi, zmniejsz liczb� pozosta�ych skok�w
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
        bulletRb.velocity = firePoint.right * bulletSpeed; // Zak�adaj�c, �e pocisk porusza si� w kierunku, w kt�rym patrzy gracz
    }

    // Zresetuj liczb� skok�w, gdy gracz dotknie ziemi
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Teren"))
        {
            jumpsRemaining = 2;
        }
    }
}