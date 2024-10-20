using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 3f;
    public float rangeShootCooldown = 0.5f; // Zmieniono na 0.5f dla szybszego testowania
    public float glowCooldown = 0.6f; // Zmieniono na 0.6f dla szybszego testowania
    public float immuneCooldown = 0.2f; // Zmieniono na 0.2f dla szybszego testowania
    public float maxAngleToShoot = 90f;
    public float maxAngleToGlow = 120f;
    public float attackDistanceThreshold1 = 1f; // Zmieniono na 1f
    public float attackDistanceThreshold2 = 4f; // Zmieniono na 4f
    public float immuneDistanceThreshold = 8f; // Nowy próg odleg³oœci dla animacji Immune

    private float rangeShootTimer = 0f;
    private float glowTimer = 0f;
    private float immuneTimer = 0f;
    private bool cooldownActive = false;
    private int animationCounter = 0;

    private Transform player;
    private Rigidbody2D rb;
    private Boss_Golem boss;

    public AudioManager audioManager;



    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss_Golem>();

        audioManager = AudioManager.GetInstance();  // Inicjalizacja AudioManager tutaj

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

        if (distanceToPlayer <= attackRange)
        {
            animator.SetTrigger("Attack");
            //audioManager.PlaySFX(audioManager.Punch); // Play attack sound
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
        else if (distanceToPlayer > attackDistanceThreshold2 && distanceToPlayer <= immuneDistanceThreshold)
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
            cooldownActive = false;
        }
    }

    public void Glow()
    {
        boss.SetGlowing(true);
        Debug.Log("Glowing from: " + rb.position);
    }

    public void Immune()
    {
        boss.SetImmune(true);
        Debug.Log("Immune from: " + rb.position);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Shoot");
        animator.ResetTrigger("Glow");
        animator.ResetTrigger("Immune");

        boss.SetGlowing(false);
        boss.SetImmune(false);
    }
}
