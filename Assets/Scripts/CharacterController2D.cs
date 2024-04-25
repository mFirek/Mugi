using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_CeilingCheck;
    [Header("Attack Animation")]
    [SerializeField] private Animator m_Animator;
    [SerializeField] private string m_AttackAnimationName = "Attack";

    const float k_GroundedRadius = .2f;
    const float k_CeilingRadius = .2f;
    private bool m_Grounded;
    private bool m_HitCeiling = false;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private Vector3 m_Velocity = Vector3.zero;

    private bool m_CanAirJump = true;
    [SerializeField] private int m_MaxAirJumps = 1;
    private int m_CurrentAirJumps = 0;

    private Rigidbody2D rb;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public UnityEvent OnLandEvent;

    [Header("Dash")]
    [SerializeField] private bool m_CanDash = true;
    [SerializeField] private float m_DashForce = 10000f;
    [SerializeField] private float m_DashDuration = 0.2f; // Czas trwania dashu
    [SerializeField] private float m_DashSmoothness = 0.1f; // P³ynnoœæ dashu

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();

        if (m_Animator == null)
            m_Animator = GetComponent<Animator>();

        m_WhatIsGround |= (1 << LayerMask.NameToLayer("Ceiling"));
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;
        m_HitCeiling = false;

        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < groundColliders.Length; i++)
        {
            if (groundColliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                    m_CanAirJump = true;
                    m_CurrentAirJumps = 0;
                }
            }
        }

        Collider2D[] ceilingColliders = Physics2D.OverlapCircleAll(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround);
        for (int i = 0; i < ceilingColliders.Length; i++)
        {
            if (ceilingColliders[i].gameObject != gameObject)
            {
                m_HitCeiling = true;
            }
        }

        if (m_Grounded && rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (!m_Grounded && rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && m_CanDash)
        {
            Dash();
        }
    }

    public void Move(float move, bool jump)
    {
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

        if (m_Grounded && jump)
        {
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
        else if (m_CanAirJump && !m_Grounded && jump && m_CurrentAirJumps < m_MaxAirJumps)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            m_CanAirJump = false;
            m_CurrentAirJumps++;
        }

        if (m_HitCeiling)
        {
            jump = false;
            m_HitCeiling = false;
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Attack()
    {
        if (m_Animator != null)
        {
            m_Animator.Play(m_AttackAnimationName);
        }
        else
        {
            Debug.LogWarning("Animator component is not assigned. Cannot play attack animation.");
        }
    }

    private void Dash()
    {
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        m_CanDash = false;

        // Tymczasowe wy³¹czenie grawitacji podczas dashowania
        rb.gravityScale = 0;

        // Zmiana prêdkoœci postaci na prêdkoœæ dashu przez okreœlony czas
        Vector2 dashVelocity = (m_FacingRight ? transform.right : -transform.right) * m_DashForce * 100000;
        float timer = 0f;

        while (timer < m_DashDuration)
        {
            m_Rigidbody2D.velocity = Vector2.Lerp(m_Rigidbody2D.velocity, dashVelocity, m_DashSmoothness);
            timer += Time.deltaTime;
            yield return null;
        }

        // Przywrócenie grawitacji po zakoñczeniu dashu
        rb.gravityScale = 1;

        m_Rigidbody2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f); // Czas odnowienia dashu
        m_CanDash = true;
    }
}
