using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 3f;
    public float RangeShoot = 30f;
    public float rangeShootCooldown = 3f; // Czas oczekiwania mi�dzy kolejnymi atakami na dystansie
    public float rangeShootTimer = 0f; // Licznik czasu dla cooldownu
    private float cooldownTimer = 0f; // Licznik czasu dla cooldownu
    private bool cooldownActive = false; // Flaga wskazuj�ca, czy cooldown jest aktywny

    public GameObject projectilePrefab; // Prefab pocisku
    public Transform shootPoint; // Miejsce, z kt�rego ma by� wystrzelony pocisk


    Transform player;
    Rigidbody2D rb;
    Boss boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();

        // Zresetuj timer cooldownu
        rangeShootTimer = 0f;
        cooldownActive = false;

        if (projectilePrefab != null)
        {
            projectilePrefab.SetActive(false);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();

        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        // Aktualizuj timer cooldownu
        rangeShootTimer += Time.deltaTime;

        // Sprawd�, czy player jest w zasi�gu ataku na dystansie
        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
            cooldownActive = true;
            // Zresetuj timer cooldownu
            cooldownTimer = 0f;
        }
        // Sprawd�, czy cooldown dla ataku na dystansie zosta� zako�czony
        else if (rangeShootTimer >= rangeShootCooldown)
        {
            animator.SetTrigger("Shoot");
            // Zresetuj timer cooldownu
            rangeShootTimer = 0f;
            cooldownActive = true;
        }
        if (cooldownActive)
        {
            cooldownTimer += Time.deltaTime;
            // Sprawd�, czy cooldown zosta� zako�czony
            if (cooldownTimer >= rangeShootCooldown)
            {
                cooldownActive = false;
            }
        }
    }
    // Wywo�ywane z animacji strza�u
    public void ShootProjectile()
    {
        // Sprawd�, czy jest miejsce, z kt�rego ma by� wystrzelony pocisk
        if (shootPoint != null && projectilePrefab != null)
        {
            // Instancjonuj prefab z miejsca strza�u
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

            // Je�li chcesz, mo�esz przekaza� dodatkowe informacje o strzale do prefabu, np. kierunek strza�u, moc itp.
            projectile.GetComponent<ProjectileScript>().SetDirection(shootPoint.forward);
           
            // Aktywuj pocisk
            projectile.SetActive(true);
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Shoot");
    }
}


