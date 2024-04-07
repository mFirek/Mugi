using UnityEngine;

public class Enemy : StateMachineBehaviour
{
    public float attackRange = 0.5f;
    public string deathTrigger = "Hit"; // Nazwa parametru wyzwalaj�cego animacj� �mierci
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

    // Metoda wywo�ywana przy kolizji z innym obiektem
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Sprawd�, czy kolizja nast�pi�a z obiektem prefabrykatu, kt�ry powoduje �mier� wroga
        if (collision.gameObject.CompareTag("Kula"))
        {
            // Je�li tak, uruchom animacj� �mierci
            animator.SetTrigger(deathTrigger);

            // Zniszcz obiekt po zako�czeniu animacji
            Destroy(animator.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
