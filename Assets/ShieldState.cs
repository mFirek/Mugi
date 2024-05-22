using UnityEngine;

public class ShieldState : StateMachineBehaviour
{
    private Animator animator;
    private Collider2D shieldCollider;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;

        // Znajdü Collider tarczy w hierarchii
        shieldCollider = animator.GetComponentInChildren<Collider2D>(true);
        if (shieldCollider != null)
        {
            ShieldCollision shieldCollision = shieldCollider.GetComponent<ShieldCollision>();
            if (shieldCollision == null)
            {
                shieldCollision = shieldCollider.gameObject.AddComponent<ShieldCollision>();
            }
            shieldCollision.OnShieldHit += HandleShieldHit;
        }
        else
        {
            Debug.LogWarning("Shield collider not found");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (shieldCollider != null)
        {
            ShieldCollision shieldCollision = shieldCollider.GetComponent<ShieldCollision>();
            if (shieldCollision != null)
            {
                shieldCollision.OnShieldHit -= HandleShieldHit;
            }
        }
        animator.ResetTrigger("ShieldDamage");
    }

    private void HandleShieldHit()
    {
        Debug.Log("Shield hit detected");
        animator.SetTrigger("ShieldDamage");
    }
}
