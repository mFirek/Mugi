//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{
//    public CharacterController2D controller;
//    public Animator animator;
//    public float runSpeed = 40f;

//    private float horizontalMove = 0f;
//    private bool jump = false;
//    private bool isAttacking = false;

//    void Update()
//    {
//        // Ruch postaci (poziomy)
//        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

//        // Ustaw animacjê biegu
//        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

//        // Skakanie
//        if (Input.GetButtonDown("Jump"))
//        {
//            jump = true;
//            animator.SetBool("IsJumping", true);
//        }

//        // Atak (Z)
//        if (Input.GetKeyDown(KeyCode.Z))
//        {
//            Attack();
//        }

//        // Dash (C)
//        if (Input.GetKeyDown(KeyCode.C))
//        {
//            controller.Dash();
//        }
//    }

//    void FixedUpdate()
//    {
//        // Ruch i skok postaci
//        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
//        jump = false;
//    }

//    public void OnLanding()
//    {
//        // Po wyl¹dowaniu wy³¹cz animacjê skoku
//        animator.SetBool("IsJumping", false);
//    }

//    private void Attack()
//    {
//        if (!isAttacking)
//        {
//            isAttacking = true;
//            animator.Play("Attack");
//            controller.Shoot();
//            Invoke("ResetAttack", 0.5f); // Resetowanie ataku po 0.5 sekundy
//        }
//    }

//    private void ResetAttack()
//    {
//        isAttacking = false;
//    }
//}
