using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform player; // Referencja do obiektu gracza
    public float moveSpeed = 5f; // Prêdkoœæ poruszania siê bossa
    private Animator animator; // Referencja do komponentu Animator

    private void Start()
    {
        // ZnajdŸ gracza na scenie na podstawie tagu "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Pobierz komponent Animator z tego obiektu
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // SprawdŸ, czy gracz zosta³ znaleziony
        if (player != null)
        {
            // Oblicz kierunek, w którym ma siê poruszaæ boss w celu œledzenia gracza
            Vector3 direction = (player.position - transform.position).normalized;

            // Oblicz now¹ pozycjê bossa na podstawie kierunku i prêdkoœci
            Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;

            // Przesuñ bossa do nowej pozycji
            transform.position = newPosition;

            // Obróæ bossa w kierunku gracza tylko wtedy, gdy gracz jest przed bossem
            if (direction.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Obrót w prawo
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0); // Obrót w lewo
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // SprawdŸ, czy kolizja nast¹pi³a z obiektem o tagu "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Wyzwól animacjê ataku
            animator.SetTrigger("Attack");
        }
    }
}
