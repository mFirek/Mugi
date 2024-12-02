using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DemonBoss : MonoBehaviour
{
    public float speed = 4f;
    public float maxHealth = 100f;
    private float currentHealth;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public TextMeshProUGUI bossHealthText;
    public Slider healthSlider; // Pasek zdrowia bossa
    public GameObject healthBarUI; // UI paska zdrowia bossa

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

        currentHealth = maxHealth; // Ustawienie pocz¹tkowego zdrowia
        healthSlider.maxValue = maxHealth; // Maksymalna wartoœæ paska zdrowia
        healthSlider.value = currentHealth;
        healthBarUI.SetActive(true); // Pokazanie paska zdrowia na pocz¹tku

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

        if (currentHealth <= 0 && isAlive)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            Debug.Log("Demon HP: " + currentHealth);

            UpdateBossHealthUI();

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void UpdateBossHealthUI()
    {
        if (bossHealthText != null)
        {
            bossHealthText.text = "Demon HP: " + Mathf.Max(0, currentHealth).ToString();
        }
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
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

        // Odtwarzanie efektu dŸwiêkowego pokonania bossa
        audioManager.PlaySFX(audioManager.bossDefeat);

        // Wy³¹czanie koliderów, które s¹ triggerami
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.isTrigger)
            {
                collider.enabled = false;
            }
        }

        // Wy³¹czenie paska zdrowia po œmierci bossa
        healthBarUI.SetActive(false);

        // Wys³anie zdarzenia analitycznego o pokonaniu bossa
        if (PlayerBossAnalytics.Instance != null)
        {
            PlayerBossAnalytics.Instance.SendBossDefeatEvent();
            Debug.Log("Zdarzenie analityczne 'boss_defeated' zosta³o wys³ane.");
        }
        else
        {
            Debug.LogError("PlayerBossAnalytics instance is null. Event not sent.");
        }

        // Dezaktywacja skryptu
        this.enabled = false;

        // Aktywacja obiektu po œmierci bossa, jeœli istnieje
        if (objectToActivateOnDeath != null)
        {
            objectToActivateOnDeath.SetActive(true);
        }

        // Zniszczenie obiektu bossa po 2 sekundach
        Destroy(gameObject, 2f);
    }


    public void PlayAttackSound()
    {
        audioManager.PlaySFX(audioManager.enemyAttack2);
    }
}
