using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Transform player;
    public float speed = 10f; // Prêdkoœæ pocisku
    public int damage = 1; // Obra¿enia zadawane przez pocisk
    public GameObject laserPrefab;
    public Transform laserSpawnPoint;
    public float fireRate = 0.5f; // Okreœl, jak czêsto boss strzela
    private float nextFireTime = 0f;
 
    public float laserLifetime = 1f; // Czas ¿ycia lasera
    private Rigidbody2D rb; // Komponent Rigidbody pocisku
    private GameObject activeLaser; // Referencja do aktywnego lasera
    void Start()
    {
      //   Pobierz komponent Rigidbody pocisku
      rb = GetComponent<Rigidbody2D>();
   // Wy³¹cz SpriteRenderer na pocz¹tku
      GetComponent<SpriteRenderer>().enabled = false;
}
    void Update()
    {
        
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
  
    // Metoda ustawiaj¹ca kierunek ruchu pocisku
    public void SetDirection(Vector3 direction)
    {
        // Jeœli chcesz, mo¿esz zmodyfikowaæ ten kod, aby nadaæ kierunek pociskowi
        // Na przyk³ad mo¿na u¿yæ tego kierunku, aby nadaæ pociskowi prêdkoœæ w odpowiednim kierunku
        // Ta implementacja zak³ada, ¿e kierunek ruchu pocisku jest znormalizowany
       // rb.velocity = direction.normalized * speed;
    }

    // Metoda wywo³ywana, gdy pocisk trafia w co
    void Shoot()
    {
        // Upewnij siê, ¿e nie strzelamy, gdy laser jest aktywny
        if (activeLaser == null)
        {
            // Pobierz wysokoœæ punktu spawnu lasera
            float laserHeight = laserSpawnPoint.position.y;
            // Skoryguj pozycjê lasera o tê wysokoœæ
            Vector3 laserSpawnPosition = new Vector3(laserSpawnPoint.position.x, laserHeight, laserSpawnPoint.position.z);

            // Wystrzel laser z poprawionej pozycji
            activeLaser = Instantiate(laserPrefab, laserSpawnPosition, laserSpawnPoint.rotation, transform);
            // Ustawienie kierunku lasera w stronê gracza (jeœli potrzebne)
            activeLaser.transform.right = (player.position - laserSpawnPosition).normalized;

            // Zniszcz laser po czasie laserLifetime
            Destroy(activeLaser, laserLifetime);
        }
    }
}

