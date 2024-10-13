using UnityEngine;

public class Boss_Demon : StateMachineBehaviour
{
    public float speed = 4f;
    public float attackRange = 2f;
    public float attackCooldown = 0.5f;

    private float attackTimer = 0f;
    private Transform player;
    private Rigidbody2D rb;
    private DemonBoss boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<DemonBoss>();

        attackTimer = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();

        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        attackTimer += Time.deltaTime;

        float distanceToPlayer = Vector2.Distance(player.position, rb.position);

        // Sprawdzanie, czy gracz jest w zasiêgu ataku
        if (distanceToPlayer <= attackRange && attackTimer >= attackCooldown)
        {
            animator.SetTrigger("Attack");
            attackTimer = 0f;
        }
        else if (distanceToPlayer > attackRange)
        {
            // Jeœli gracz wyjdzie z zasiêgu ataku, resetujemy atak
            animator.ResetTrigger("Attack");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Resetujemy atak przy wyjœciu ze stanu, jeœli jest ustawiony
        animator.ResetTrigger("Attack");
    }
}


