using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;

    bool isJumping = false; // Dodaj zmienn¹ do sprawdzania, czy postaæ jest w trakcie skoku.


    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump") && !isJumping) // Dodaj warunek sprawdzaj¹cy, czy postaæ nie jest w trakcie skoku.
        {
            jump = true;
            animator.SetBool("IsJumping", true);
            isJumping = true; // Ustaw zmienn¹, aby oznaczyæ, ¿e postaæ jest w trakcie skoku.
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
        isJumping = false; // Ustaw zmienn¹, aby oznaczyæ, ¿e postaæ nie jest ju¿ w trakcie skoku.
    }


    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}