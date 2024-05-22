using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 3f;
    public float rangeShootCooldown = 3f;
    public float immuneRange = 10f; // Zasiêg, w jakim mo¿na u¿yæ animacji Immune
    public float immuneCooldown = 5f; // Cooldown dla animacji Immune
    private float rangeShootTimer = 0f;
    private float cooldownTimer = 0f;
    private bool cooldownActive = false;
    private bool useShootAnimation = true; // Prze³¹cznik miêdzy animacjami
    private bool immuneActive = false; // Flaga wskazuj¹ca, czy animacja Immune jest aktywna
    private float immuneTimer = 0f; // Licznik czasu dla cooldownu Immune

    public Transform firePoint;
    public GameObject bulletPrefab;

    private Transform player;
    private Rigidbody2D rb;
    private Boss boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();

        rangeShootTimer = 0f;
        cooldownActive = false;

        // Ensure firePoint is correctly assigned
        if (firePoint == null)
        {
            firePoint = animator.transform.Find("FirePoint");
        }

        if (firePoint == null)
        {
            Debug.LogError("FirePoint not found");
        }
        else
        {
            Debug.Log("FirePoint found: " + firePoint.position);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();

        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        rangeShootTimer += Time.deltaTime;

        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
            cooldownActive = true;
            cooldownTimer = 0f;
        }
        else if (rangeShootTimer >= rangeShootCooldown)
        {
            if (useShootAnimation)
            {
                animator.SetTrigger("Shoot");
                Shoot();
            }
            else
            {
                if (Vector2.Distance(player.position, rb.position) >= immuneRange)
                {
                    animator.SetTrigger("Immune");
                    immuneActive = true;
                    immuneTimer = 0f;
                }
                else
                {
                    animator.SetTrigger("Glow");
                    Glow();
                }
            }

            rangeShootTimer = 0f;
            cooldownActive = true;
            useShootAnimation = !useShootAnimation; // Prze³¹cz miêdzy animacjami
        }

        if (!cooldownActive && !immuneActive && immuneTimer >= immuneCooldown)
        {
            animator.SetTrigger("Immune");
            immuneActive = true;
            immuneTimer = 0f;
        }

        if (cooldownActive)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= rangeShootCooldown)
            {
                cooldownActive = false;
            }
        }

        if (immuneActive)
        {
            immuneTimer += Time.deltaTime;
            if (immuneTimer >= immuneCooldown)
            {
                immuneActive = false;
            }
        }
    }

    public void Shoot()
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint not assigned");
            return;
        }

        Vector2 lookDir = player.position - firePoint.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log("Shooting from: " + firePoint.position); // Debugging position

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        rbBullet.AddForce(firePoint.right * 20f, ForceMode2D.Impulse); // Adjusted force for better shooting
    }

    public void Glow()
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint not assigned");
            return;
        }

        boss.SetGlowing(true); // Ustaw, ¿e boss œwieci

        Debug.Log("Glowing from: " + firePoint.position); // Debugging position
        // Tu dodaj funkcjonalnoœæ dla animacji "Glow"
    }

    public void Immune()
    {
        // Dodaj funkcjonalnoœæ dla animacji "Immune"
        Debug.Log("Immune");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Shoot");
        animator.ResetTrigger("Glow");
        animator.ResetTrigger("Immune");
        boss.SetGlowing(false); // Resetuj, ¿e boss przesta³ œwieciæ
        immuneActive = false; // Resetuj flagê Immune
    }
}
