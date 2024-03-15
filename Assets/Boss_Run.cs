using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 3f;
    public float RangeShoot = 30f;
    public float rangeShootCooldown = 3f; // Czas oczekiwania miêdzy kolejnymi atakami na dystansie
    public float rangeShootTimer = 0f; // Licznik czasu dla cooldownu
    private bool cooldownActive = false; // Flaga wskazuj¹ca, czy cooldown jest aktywny


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

        // SprawdŸ, czy player jest w zasiêgu ataku na dystansie
        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
        }
        // SprawdŸ, czy cooldown dla ataku na dystansie zosta³ zakoñczony
        else if (rangeShootTimer >= rangeShootCooldown)
        {
            animator.SetTrigger("Shoot");
            // Zresetuj timer cooldownu
            rangeShootTimer = 0f;
            cooldownActive = true;
        }
        if (cooldownActive)
        {
            rangeShootTimer += Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Shoot");
    }
}
