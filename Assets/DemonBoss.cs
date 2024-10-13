using UnityEngine;
using TMPro; // Dodaj to, aby korzystaæ z TextMeshPro

public class DemonBoss : MonoBehaviour
{
    public float speed = 4f;  // Demon is faster than Golem
    public float health = 100f;
    public float attackRange = 2f;
    public float attackCooldown = 1f; // Centralizacja cooldownu w jednym miejscu
    public TextMeshProUGUI bossHealthText;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isAlive = true;
    private bool isAttacking = false; // Zmienna kontroluj¹ca, czy demon jest w trakcie ataku
    private float attackTimer = 0f; // Timer do cooldownu ataku

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Zaktualizuj pocz¹tkowy tekst HP
        UpdateBossHealthUI();
    }

    void Update()
    {
        if (!isAlive) return;

        LookAtPlayer();

        float distanceToPlayer = Vector2.Distance(player.position, rb.position);

        // Zaktualizuj timer cooldownu
        attackTimer += Time.deltaTime;

        // Atakuj tylko wtedy, gdy spe³nione s¹ warunki
        if (distanceToPlayer <= attackRange && attackTimer >= attackCooldown && !isAttacking)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }

        // Check if health falls below 0 and trigger death if so
        if (health <= 0 && isAlive)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        // Zapobiegaj spadaniu HP poni¿ej 0
        if (health > 0)
        {
            health -= damage;
            Debug.Log("Demon HP: " + health);

            // Zaktualizuj UI po utracie zdrowia
            UpdateBossHealthUI();

            // Trigger death if health drops to 0 or below
            if (health <= 0)
            {
                Die();
            }
        }
    }

    // Aktualizacja tekstu UI HP
    void UpdateBossHealthUI()
    {
        if (bossHealthText != null)
        {
            bossHealthText.text = "Demon HP: " + Mathf.Max(0, health).ToString(); // Zapobiegaj wyœwietlaniu ujemnych wartoœci
        }
    }

    public void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        if (direction.x > 0 && transform.localScale.x < 0)
        {
            // Patrz w prawo
            Flip();
        }
        else if (direction.x < 0 && transform.localScale.x > 0)
        {
            // Patrz w lewo
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
        isAttacking = true; // Ustaw flagê ataku
        animator.SetTrigger("Attack");
        attackTimer = 0f; // Resetuj timer po ataku
    }

    // Funkcja wywo³ywana po zakoñczeniu animacji ataku
    public void EndAttack()
    {
        isAttacking = false; // Ustaw flagê ataku na false, aby pozwoliæ na nowy atak
    }

    // Dodajemy tutaj logikê tracenia zdrowia po kontakcie z obiektem o tagu "Kula"
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Kula"))
        {
            TakeDamage(1f); // Ustal, ¿e demon traci 1 punkt zdrowia
        }
    }

    void Die()
    {
        isAlive = false;
        animator.SetTrigger("Die");
        Debug.Log("Demon zgin¹³!");

        // Additional death logic
        GetComponent<Collider2D>().enabled = false; // Wy³¹czenie kolizji
        this.enabled = false; // Wy³¹czenie skryptu, aby demon przesta³ siê poruszaæ i reagowaæ
        Destroy(gameObject, 2f); // Zniszczenie obiektu po 2 sekundach (opcjonalnie)
    }
}
