using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_RunJump : StateMachineBehaviour
{
    public float speed = 2f;
    public float jumpForce = 5f;
    public float attackRange = 1.5f;
    public float jumpTriggerDistance = 3f;
    public float jumpDistance = 5f; // Odleg³oœæ, z jakiej boss mo¿e skakaæ na platformê

    private List<Transform> platforms = new List<Transform>(); // Lista platform

    Transform player;
    Rigidbody2D rb;
    Boss boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();

        // ZnajdŸ wszystkie platformy w scenie
        GameObject[] platformObjects = GameObject.FindGameObjectsWithTag("platform");
        platforms.Clear();
        foreach (GameObject platformObject in platformObjects)
        {
            platforms.Add(platformObject.transform);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();

     
      
            animator.SetTrigger("Attack");
    }

  

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Jump");
    }
}
