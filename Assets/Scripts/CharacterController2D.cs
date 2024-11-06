using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_CeilingCheck;

    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private bool m_Grounded;
    private bool m_CanAirJump = true;
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Jump Settings")]
    [SerializeField] private int m_MaxAirJumps = 2;
    private int m_CurrentJumps = 0;

    [Header("Animator")]
    public Animator animator;

    [Header("Attack Settings")]
    [SerializeField] private string m_AttackAnimationName = "Attack";
    public GameObject bulletPrefab;
    public Transform firePointR;
    public Transform firePointL;
    public float bulletSpeed = 1f;
    private Vector2 lastDirection = Vector2.right;

    public float runSpeed = 10f;
    private float horizontalMove = 0f;
    private bool jump = false;
    private bool isFalling = false;

    AudioManager audioManager;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Update()
    {
        if (DialogueManager.GetInstance() != null && DialogueManager.GetInstance().dialogueIsPlaying)
        {
            horizontalMove = 0;
            animator.SetFloat("Speed", 0);
            return;
        }

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump") && (m_Grounded || m_CurrentJumps < m_MaxAirJumps) && !animator.GetBool("isAttacking"))
        {
            audioManager.PlaySFX(audioManager.Jump);
            jump = true;
            animator.SetBool("IsJumping", true);
            isFalling = false;
        }

        if (Input.GetKeyDown(KeyCode.Z) && !animator.GetBool("isAttacking"))
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance() != null && DialogueManager.GetInstance().dialogueIsPlaying)
        {
            Move(0, false);
            return;
        }

        Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;

        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, 0.01f, m_WhatIsGround);
        foreach (var collider in groundColliders)
        {
            if (collider.gameObject != gameObject && m_GroundCheck.position.y > collider.bounds.center.y)
            {
                m_Grounded = true;
                if (!wasGrounded)
                {
                    m_CanAirJump = true;
                    m_CurrentJumps = 0;
                    animator.SetBool("IsJumping", false);
                    animator.SetBool("IsFalling", false); // Wy³¹czenie animacji „Falling” po l¹dowaniu
                }
                break;
            }
        }

        // Logika animacji spadania
        if (!m_Grounded && m_Rigidbody2D.velocity.y < 0)
        {
            if (animator.GetBool("IsJumping")) // Jeœli postaæ zaczyna opadaæ po skoku
            {
                animator.SetBool("IsJumping", false); // Wy³¹czenie „IsJumping”
                animator.SetBool("IsFalling", true); // Aktywacja „IsFalling”
            }
            else if (!animator.GetBool("IsFalling")) // Jeœli postaæ spada bez skoku (np. z platformy)
            {
                animator.SetBool("IsFalling", true);
            }
        }
        else if (m_Grounded) // Gdy postaæ dotknie ziemi
        {
            animator.SetBool("IsFalling", false);
        }
    }



    private void Move(float move, bool jump)
    {
        if (DialogueManager.GetInstance() != null && DialogueManager.GetInstance().dialogueIsPlaying)
        {
            m_Rigidbody2D.velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
            return;
        }

        if (m_Grounded || m_AirControl)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip();
            }
        }

        if (jump)
        {
            if (m_Grounded)
            {
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                m_CurrentJumps++;
            }
            else if (m_CurrentJumps < m_MaxAirJumps)
            {
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                m_CurrentJumps++;
            }
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        lastDirection = m_FacingRight ? Vector2.right : Vector2.left;
    }

    private void Attack()
    {
        if (!CanPerformAction())
        {
            return;
        }

        audioManager.PlaySFX(audioManager.Attack);
        animator.SetBool("isAttacking", true);  // Ustawienie flagi ataku
        animator.Play(m_AttackAnimationName);

        Shoot();
    }

    public void EndAttack()
    {
        animator.SetBool("isAttacking", false);

        // Powrót do animacji skoku lub spadania, jeœli gracz jest w powietrzu
        if (!m_Grounded)
        {
            if (m_Rigidbody2D.velocity.y > 0)
            {
                animator.SetBool("IsJumping", true);
                animator.SetBool("IsFalling", false);
            }
            else
            {
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", true);
            }
        }
    }

    private void Shoot()
    {
        if (!CanPerformAction())
        {
            return;
        }

        Transform firePoint = lastDirection == Vector2.right ? firePointR : firePointL;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = lastDirection * bulletSpeed;
    }

    private bool CanPerformAction()
    {
        return DialogueManager.GetInstance() == null || !DialogueManager.GetInstance().dialogueIsPlaying;
    }
}
