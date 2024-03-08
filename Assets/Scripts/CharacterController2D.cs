using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_CeilingCheck;
    [Header("Attack Animation")]
    [SerializeField] private Animator m_Animator; // Animator komponent do obs³ugi animacji.
    [SerializeField] private string m_AttackAnimationName = "Attack"; // Nazwa animacji ataku.

    const float k_GroundedRadius = .2f;
    private bool m_Grounded;
    const float k_CeilingRadius = .2f;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private Vector3 m_Velocity = Vector3.zero;



    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private bool m_CanAirJump = true; // Whether the player can perform an air jump.
    [SerializeField] private int m_MaxAirJumps = 1; // Maximum number of air jumps allowed.
    private int m_CurrentAirJumps = 0; // Current number of air jumps performed.

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
        if (m_Animator == null)
        {
            m_Animator = GetComponent<Animator>(); // Spróbuj znaleŸæ Animator na tym samym obiekcie.
        }
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                    m_CanAirJump = true; // Reset air jumps when landing.
                    m_CurrentAirJumps = 0;

                }
            }
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
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f); // Zero out vertical velocity before air jump.
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            m_CanAirJump = false;
            m_CurrentAirJumps++;

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
        // SprawdŸ, czy Animator zosta³ przypisany.
        if (m_Animator != null)
        {
            // Odtwórz animacjê ataku, jeœli Animator istnieje.
            m_Animator.Play(m_AttackAnimationName);
        }
        else
        {
            Debug.LogWarning("Animator component is not assigned. Cannot play attack animation.");
        }
    }
    private IEnumerator ReturnToIdleAfterAttack()
    {
        // Poczekaj, a¿ animacja ataku siê zakoñczy.
        yield return new WaitForSeconds(m_Animator.GetCurrentAnimatorStateInfo(0).length);

        // Wróæ do animacji idle.
        m_Animator.Play("Idle"); // Zak³adaj¹c, ¿e masz animacjê idle o nazwie "Idle".
    }
}
