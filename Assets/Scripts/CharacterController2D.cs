using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    // ============================
    // Zmienne dotycz�ce ruchu gracza
    // ============================

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

    // ============================
    // Zmienne do skakania i double jumpa
    // ============================

    [Header("Jump Settings")]
    [SerializeField] private int m_MaxAirJumps = 2; // Liczba dodatkowych skok�w
    private int m_CurrentJumps = 0; // Aktualna liczba wykonanych skok�w

    // ============================
    // Zmienne do obs�ugi animacji
    // ============================

    [Header("Animator")]
    public Animator animator;

    // ============================
    // Zmienne dotycz�ce ataku
    // ============================

    [Header("Attack Settings")]
    [SerializeField] private string m_AttackAnimationName = "Attack";
    public GameObject bulletPrefab;
    public Transform firePointR;
    public Transform firePointL;
    public float bulletSpeed = 1f;
    private Vector2 lastDirection = Vector2.right;

    // ============================
    // Zmienne dotycz�ce ruchu gracza
    // ============================

    public float runSpeed = 10f;  // Pr�dko�� biegania
    private float horizontalMove = 0f;
    private bool jump = false;

    // ============================
    // Tagi resetuj�ce animacj� skoku
    // ============================

    [Header("Reset Jump Animation Tags")]
    [SerializeField] private string[] resetJumpTags; // Tagi do resetowania animacji skoku

    // ============================
    // AudioManager
    // ============================

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
        // ============================
        // Sprawdzenie, czy dialog trwa
        // ============================
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        // ============================
        // Obs�uga ruchu
        // ============================
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // ============================
        // Skakanie
        // ============================
        if (Input.GetButtonDown("Jump") && (m_Grounded || m_CurrentJumps < m_MaxAirJumps))
        {
            audioManager.PlaySFX(audioManager.Jump); // Odtworzenie d�wi�ku skoku
            jump = true;
            animator.SetBool("IsJumping", true); // Ustawienie animacji skoku
        }

        // ============================
        // Obs�uga ataku (Z)
        // ============================
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        // ============================
        // Sprawdzenie, czy dialog trwa
        // ============================
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        // ============================
        // Logika poruszania si� postaci
        // ============================
        Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;

        // ============================
        // Sprawdzenie, czy posta� jest na ziemi
        // ============================
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, 0.2f, m_WhatIsGround);
        for (int i = 0; i < groundColliders.Length; i++)
        {
            if (groundColliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                {
                    m_CanAirJump = true;
                    m_CurrentJumps = 0; // Resetowanie licznika skok�w po wyl�dowaniu
                    animator.SetBool("IsJumping", false); // Resetowanie animacji skoku po l�dowaniu
                }
            }
        }
    }

    // ============================
    // Pomocnicza metoda do sprawdzenia, czy mo�na wykona� akcj�
    // ============================
    private bool CanPerformAction()
    {
        return !DialogueManager.GetInstance().dialogueIsPlaying;
    }

    public void Move(float move, bool jump)
    {
        if (!CanPerformAction())
        {
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
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f); // Zresetowanie pr�dko�ci w osi Y
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce)); // Dodanie si�y skoku
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

        // Logika ataku
        audioManager.PlaySFX(audioManager.Attack); // Odtworzenie d�wi�ku ataku
        animator.Play(m_AttackAnimationName); // Uruchomienie animacji ataku
        Shoot(); // Wystrza�
    }

    private void Shoot()
    {
        if (!CanPerformAction())
        {
            return;
        }

        // Logika strzelania
        Transform firePoint = lastDirection == Vector2.right ? firePointR : firePointL;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = lastDirection * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Resetowanie animacji skoku przy kolizji
        foreach (string tag in resetJumpTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                animator.SetBool("IsJumping", false); // Resetowanie animacji skoku
                break; // Nie sprawdzaj dalej po znalezieniu pasuj�cego tagu
            }
        }
    }
}

