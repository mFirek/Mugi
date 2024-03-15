using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform player; // Referencja do obiektu gracza
    public float moveSpeed = 5f; // Pr�dko�� poruszania si� bossa
    private Animator animator; // Referencja do komponentu Animator

    private void Start()
    {
        // Znajd� gracza na scenie na podstawie tagu "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Pobierz komponent Animator z tego obiektu
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Sprawd�, czy gracz zosta� znaleziony
        if (player != null)
        {
            // Oblicz kierunek, w kt�rym ma si� porusza� boss w celu �ledzenia gracza
            Vector3 direction = (player.position - transform.position).normalized;

            // Oblicz now� pozycj� bossa na podstawie kierunku i pr�dko�ci
            Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;

            // Przesu� bossa do nowej pozycji
            transform.position = newPosition;

            // Obr�� bossa w kierunku gracza tylko wtedy, gdy gracz jest przed bossem
            if (direction.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Obr�t w prawo
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0); // Obr�t w lewo
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Sprawd�, czy kolizja nast�pi�a z obiektem o tagu "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Wyzw�l animacj� ataku
            animator.SetTrigger("Attack");
        }
    }
}
