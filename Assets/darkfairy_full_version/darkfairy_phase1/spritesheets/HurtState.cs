using UnityEngine;

public class HurtState : StateMachineBehaviour
{
    private Animator animator;
    

    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        this.animator = animator;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Tylko gdy stan jest Idle, sprawdzamy kolizje
        
            // Sprawdzamy, czy obiekt o tagu "Kula" wchodzi w kolizj� z graczem
            Collider2D[] colliders = Physics2D.OverlapCircleAll(animator.transform.position, 0.5f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Kula"))
                {
                    // Je�li obiekt o tagu "Kula" wchodzi w kolizj� z graczem, uruchamiamy animacj� "Hurt"
                    animator.SetTrigger("Hurt");
                  
                }
            }
        
    }
}
