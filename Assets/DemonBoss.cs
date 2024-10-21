using UnityEngine;
using TMPro;

public class DemonBoss : MonoBehaviour
{
    public float speed = 4f;
    public float health = 100f;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public TextMeshProUGUI bossHealthText;

    // Dodaj referencjê do obiektu, który ma siê pojawiæ po œmierci bossa
    public GameObject objectToActivateOnDeath;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isAlive = true;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.GetInstance();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        UpdateBossHealthUI();
    }

    void Update()
    {
        if (!isAlive) return;

        LookAtPlayer();
        float distanceToPlayer = Vector2.Distance(player.position, rb.position);
        attackTimer += Time.deltaTime;

        if (distanceToPlayer <= attackRange && attackTimer >= attackCooldown && !isAttacking)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }

        if (health <= 0 && isAlive)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            Debug.Log("Demon HP: " + health);

            UpdateBossHealthUI();

            if (health <= 0)
            {
                Die();
            }
        }
    }

    void UpdateBossHealthUI()
    {
        if (bossHealthText != null)
        {
            bossHealthText.text = "Demon HP: " + Mathf.Max(0, health).ToString();
        }
    }

    public void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        if (direction.x > 0 && transform.localScale.x < 0)
        {
            Flip();
        }
        else if (direction.x < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void ChasePlayer()
    {
        animator.SetBool("isWalking", true);
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    void AttackPlayer()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        attackTimer = 0f;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Kula"))
        {
            TakeDamage(1f);
        }
    }

    void Die()
    {
        isAlive = false;
        animator.SetTrigger("Die");
        Debug.Log("Demon zgin¹³!");

        AudioManager.GetInstance().PlaySFX(audioManager.bossDefeat);

        // Wy³¹czenie tylko koliderów, które s¹ IsTrigger
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.isTrigger)
            {
                collider.enabled = false;
            }
        }

        this.enabled = false;

        // Aktywacja obiektu po œmierci bossa
        if (objectToActivateOnDeath != null)
        {
            objectToActivateOnDeath.SetActive(true);
        }

        Destroy(gameObject, 2f);
    }

    public void PlayAttackSound()
    {
        audioManager.PlaySFX(audioManager.enemyAttack2);
    }
}
