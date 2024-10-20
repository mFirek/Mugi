using UnityEngine;

public class Enemy : StateMachineBehaviour
{
    public float attackRange = 0.5f;
    public string deathTrigger = "Hit"; // Nazwa parametru wyzwalaj¹cego animacjê œmierci
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private AudioManager audioManager; // Dodajemy AudioManager

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponentInChildren<Rigidbody2D>();
        audioManager = AudioManager.GetInstance(); // Pobranie instancji AudioManager


    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
            audioManager.PlaySFX(audioManager.enemyAttack2); // Odtwórz dŸwiêk ataku
        }
    }

    // Metoda odtwarzaj¹ca dŸwiêk ataku


    // Metoda wywo³ywana przy kolizji z innym obiektem
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Kula"))
        {
            animator.SetTrigger(deathTrigger);
            Destroy(animator.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}

