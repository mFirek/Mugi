using System.Collections;
using UnityEngine;

public class WugiBoss : MonoBehaviour
{
    public Animator animator;
    public float attackRange = 5f;      // Zasi�g ataku wr�cz
    public float specialAttackRange = 10f; // Zasi�g ataku specjalnego
    public float speed = 2f;
    public Transform target;

    public GameObject projectile;
    public Transform firePoint;

    private bool isDead = false;
    private bool canSpecialAttack = true;
    public float specialAttackCooldown = 1f; // Czas odnowienia ataku specjalnego

    AudioManager audioManager;

    void Start()
    {
        // Znajduje gracza na podstawie tagu "Player"
        target = GameObject.FindGameObjectWithTag("Player").transform;
        audioManager = AudioManager.GetInstance();
    }
    public void PlayAttackSoundWugi()
    {
        audioManager.PlaySFX(audioManager.WugiSpell);
    }
    public void PlayAttackSoundWugi2()
    {
        audioManager.PlaySFX(audioManager.WugiAttack);
    }
    void Update()
    {
        if (isDead) return;

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRange)
        {
            // Atak wr�cz
            animator.SetTrigger("attack");
        }
        else if (distanceToTarget <= specialAttackRange && canSpecialAttack)
        {
            // Wyzwalanie animacji ataku specjalnego
            animator.SetTrigger("specialAttack");
            canSpecialAttack = false; // Blokujemy ponowne wywo�anie ataku specjalnego
            StartCoroutine(SpecialAttackCooldown());
        }
        else
        {
            // �ciganie gracza, je�li nie jest w zasi�gu ataku specjalnego
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        // Boss porusza si� w kierunku gracza
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Zmiana kierunku, je�li gracz znajduje si� po prawej lub lewej stronie bossa
        FacePlayer();
    }

    void FacePlayer()
    {
        // Je�li gracz jest po lewej stronie, boss patrzy w lewo (x = -1)
        // Je�li gracz jest po prawej stronie, boss patrzy w prawo (x = 1)
        if (target.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1); // patrzy w prawo
        else if (target.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1); // patrzy w lewo
    }

    void ShootProjectile()
    {
        // Tworzymy pocisk
        GameObject firedProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);

        // Pobieramy SpriteRenderer pocisku
        SpriteRenderer projectileSpriteRenderer = firedProjectile.GetComponent<SpriteRenderer>();

        // Sprawdzamy, w kt�r� stron� patrzy boss
        if (transform.localScale.x > 0) // Boss patrzy w prawo
        {
            firedProjectile.transform.Rotate(0, 0, -90); // Rotacja pocisku, je�li boss patrzy w prawo
            projectileSpriteRenderer.flipX = false;
        }
        else if (transform.localScale.x < 0) // Boss patrzy w lewo
        {
            firedProjectile.transform.Rotate(0, 0, 90); // Rotacja pocisku w lewo
            projectileSpriteRenderer.flipX = true; // Flip pocisku, gdy boss patrzy w lewo
        }

        // Obliczamy kierunek w stron� gracza
        Vector2 direction = (target.position - firePoint.position).normalized;

        // Pobieramy Rigidbody2D pocisku i dodajemy si�� w kierunku gracza
        Rigidbody2D rb = firedProjectile.GetComponent<Rigidbody2D>();
        float projectileForce = 10f; // Mo�esz dostosowa� si�� pocisku
        rb.AddForce(direction * projectileForce, ForceMode2D.Impulse);
    }

    IEnumerator SpecialAttackCooldown()
    {
        yield return new WaitForSeconds(specialAttackCooldown);
        canSpecialAttack = true; // Reset flagi po cooldownie
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            animator.SetTrigger("die");
        }
    }
}
