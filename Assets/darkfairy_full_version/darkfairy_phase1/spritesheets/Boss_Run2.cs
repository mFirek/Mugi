using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run2 : StateMachineBehaviour
{
    public float speed = 1f;
    public float attackRange = 1f;
    public float specialAttackRange = 2f; // Zakres specjalnego ataku
    public float shieldRange = 3f; // Zakres użycia tarczy

    Transform player;
    Rigidbody2D rb;
    Boss boss;



    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();




        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        float distanceToPlayer = Vector2.Distance(player.position, rb.position);

        // Sprawdzamy, czy tarcza jest gotowa do użycia
        if (Time.time >= boss.lastShieldTime + boss.shieldCooldown && distanceToPlayer <= shieldRange)
        {
            animator.SetTrigger("Shield");
            boss.lastShieldTime = Time.time;
        }
        // Sprawdzamy, czy specjalny atak jest gotowy i boss jest w zasięgu specjalnego ataku
        else if (Time.time >= boss.lastSpecialAttackTime + boss.specialAttackCooldown && distanceToPlayer <= specialAttackRange)
        {
            animator.SetTrigger("SpecialAttack");
            boss.lastSpecialAttackTime = Time.time;
        }
        // Jeśli specjalny atak nie jest gotowy, ale boss jest w zasięgu normalnego ataku
        else if (distanceToPlayer <= attackRange)
        {
            animator.SetTrigger("Attack");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("SpecialAttack");
        animator.ResetTrigger("Shield");
    }
}
