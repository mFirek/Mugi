using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 3f;
    public float rangeShootCooldown = 3f;
    public float glowCooldown = 5f; // Czas odnowienia animacji "Glow"
    public float immuneCooldown = 7f; // Czas odnowienia animacji "Immune"
    public float maxAngleToShoot = 90f; // Maksymalny k�t, pod jakim boss mo�e strzela�
    public float maxAngleToGlow = 120f; // Maksymalny k�t, pod jakim boss mo�e u�y� "Glow"
    public float attackDistanceThreshold1 = 5f; // Dystans, od kt�rego zaczyna si� pierwszy atak
    public float attackDistanceThreshold2 = 10f; // Dystans, od kt�rego zaczyna si� drugi atak

    private float rangeShootTimer = 0f;
    private float glowTimer = 0f;
    private float immuneTimer = 0f;
    private bool cooldownActive = false;
    private int animationCounter = 0; // Zmienna do prze��czania animacji

    private Transform player;
    private Rigidbody2D rb;
    private Boss_Golem boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss_Golem>();

        rangeShootTimer = 0f;
        glowTimer = 0f;
        immuneTimer = 0f;
        cooldownActive = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();

        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        rangeShootTimer += Time.deltaTime;
        glowTimer += Time.deltaTime;
        immuneTimer += Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(player.position, rb.position);
        Vector2 playerPosition2D = new Vector2(player.position.x, player.position.y);

        // Sprawdzenie warunk�w atak�w w zale�no�ci od odleg�o�ci do gracza
        if (distanceToPlayer <= attackRange)
        {
            animator.SetTrigger("Attack");
            cooldownActive = true;
        }
        else if (distanceToPlayer > attackRange && distanceToPlayer <= attackDistanceThreshold1)
        {
            float angleToPlayer = Vector2.Angle(rb.transform.right, playerPosition2D - rb.position);
            if (angleToPlayer <= maxAngleToGlow && glowTimer >= glowCooldown)
            {
                animator.SetTrigger("Glow");
                glowTimer = 0f;
                cooldownActive = true;
            }
        }
        else if (distanceToPlayer > attackDistanceThreshold1 && distanceToPlayer <= attackDistanceThreshold2)
        {
            if (rangeShootTimer >= rangeShootCooldown)
            {
                float angleToPlayer = Vector2.Angle(rb.transform.right, playerPosition2D - rb.position);
                if (angleToPlayer <= maxAngleToShoot)
                {
                    animator.SetTrigger("Shoot");
                    rangeShootTimer = 0f;
                    cooldownActive = true;
                }
            }
        }
        else if (distanceToPlayer > attackDistanceThreshold2)
        {
            if (immuneTimer >= immuneCooldown)
            {
                animator.SetTrigger("Immune");
                immuneTimer = 0f;
                cooldownActive = true;
            }
        }

        if (cooldownActive)
        {
            // Reset cooldowns after action
            cooldownActive = false;
        }
    }

    public void Glow()
    {
        boss.SetGlowing(true); // Ustaw, �e boss �wieci
        Debug.Log("Glowing from: " + rb.position); // Debugging position
        // Tu dodaj funkcjonalno�� dla animacji "Glow"
    }

    public void Immune()
    {
        boss.SetImmune(true); // Ustaw, �e boss jest odporny
        Debug.Log("Immune from: " + rb.position); // Debugging position
        // Tu dodaj funkcjonalno�� dla animacji "Immune"
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Shoot");
        animator.ResetTrigger("Glow");
        animator.ResetTrigger("Immune");
        boss.SetGlowing(false); // Resetuj, �e boss przesta� �wieci�
        boss.SetImmune(false); // Resetuj, �e boss przesta� by� odporny
    }
}
