using UnityEngine;

public class Enemy : StateMachineBehaviour
{
    public float attackRange = 0.5f;
    public string deathTrigger = "Hit"; // Nazwa parametru wyzwalaj¹cego animacjê œmierci
    Transform player;
    Rigidbody2D rb;
    Animator animator;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponentInChildren<Rigidbody2D>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
        }
    }

    // Metoda wywo³ywana przy kolizji z innym obiektem
    void OnCollisionEnter2D(Collision2D collision)
    {
        // SprawdŸ, czy kolizja nast¹pi³a z obiektem prefabrykatu, który powoduje œmieræ wroga
        if (collision.gameObject.CompareTag("Kula"))
        {
            // Jeœli tak, uruchom animacjê œmierci
            animator.SetTrigger(deathTrigger);

            // Zniszcz obiekt po zakoñczeniu animacji
            Destroy(animator.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
