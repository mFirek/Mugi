using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public GameObject bulletPrefab;
    public Transform firePointR;
    public Transform firePointL;
    public float bulletSpeed = 1f;
    public float runSpeed = 10f;
    private Vector2 lastDirection = Vector2.right;
    float horizontalMove = 0f;
    bool jump = false;

    void Update()
    {
        // Sprawd�, czy dialog jest aktywny
        bool isDialogueActive = DialogueManager.GetInstance() != null && DialogueManager.GetInstance().dialogueIsPlaying;

        // Tylko aktualizuj ruch i animacje, je�li dialog nie jest aktywny
        if (!isDialogueActive)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
                animator.SetBool("IsJumping", true);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                animator.Play("Attack", -1, 0f);
                Shoot();
            }
        }
        else
        {
            // Ustaw animacj� na idle podczas dialogu
            animator.SetFloat("Speed", 0);
            animator.SetBool("IsJumping", false);
        }
    }

    void Shoot()
    {

        // Sprawd�, czy dialog jest aktywny
        if (DialogueManager.GetInstance() != null && DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return; // Nie wykonuj strza�u, je�li dialog jest aktywny
        }

        // Wybieramy odpowiedni firePoint w zale�no�ci od ostatniego kierunku patrzenia
        Transform firePoint = lastDirection == Vector2.right ? firePointR : firePointL;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // Ustalamy kierunek strza�u na podstawie kierunku patrzenia
        Vector2 shootDirection = lastDirection;

        if (horizontalMove == 0)
        {
            // Je�li gracz stoi w miejscu, kierunek patrzenia zostaje zachowany
            shootDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            
        }
        else if (horizontalMove < 0)
        {
            // Je�li gracz porusza si� w lewo, kierunek strza�u r�wnie� zostaje zmieniony na lewo
            shootDirection = Vector2.left;
        }

        bulletRb.velocity = shootDirection * bulletSpeed;
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {
        // Je�li dialog jest aktywny, zablokuj ruch
        if (DialogueManager.GetInstance() == null || !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        }
        jump = false;
    }
}
