using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Transform player;
    public float speed = 10f; // Pr�dko�� pocisku
    public int damage = 1; // Obra�enia zadawane przez pocisk
    public GameObject laserPrefab;
    public Transform laserSpawnPoint;
    public float fireRate = 0.5f; // Okre�l, jak cz�sto boss strzela
    private float nextFireTime = 0f;
 
    public float laserLifetime = 1f; // Czas �ycia lasera
    private Rigidbody2D rb; // Komponent Rigidbody pocisku
    private GameObject activeLaser; // Referencja do aktywnego lasera
    void Start()
    {
      //   Pobierz komponent Rigidbody pocisku
      rb = GetComponent<Rigidbody2D>();
   // Wy��cz SpriteRenderer na pocz�tku
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
  
    // Metoda ustawiaj�ca kierunek ruchu pocisku
    public void SetDirection(Vector3 direction)
    {
        // Je�li chcesz, mo�esz zmodyfikowa� ten kod, aby nada� kierunek pociskowi
        // Na przyk�ad mo�na u�y� tego kierunku, aby nada� pociskowi pr�dko�� w odpowiednim kierunku
        // Ta implementacja zak�ada, �e kierunek ruchu pocisku jest znormalizowany
       // rb.velocity = direction.normalized * speed;
    }

    // Metoda wywo�ywana, gdy pocisk trafia w co
    void Shoot()
    {
        // Upewnij si�, �e nie strzelamy, gdy laser jest aktywny
        if (activeLaser == null)
        {
            // Pobierz wysoko�� punktu spawnu lasera
            float laserHeight = laserSpawnPoint.position.y;
            // Skoryguj pozycj� lasera o t� wysoko��
            Vector3 laserSpawnPosition = new Vector3(laserSpawnPoint.position.x, laserHeight, laserSpawnPoint.position.z);

            // Wystrzel laser z poprawionej pozycji
            activeLaser = Instantiate(laserPrefab, laserSpawnPosition, laserSpawnPoint.rotation, transform);
            // Ustawienie kierunku lasera w stron� gracza (je�li potrzebne)
            activeLaser.transform.right = (player.position - laserSpawnPosition).normalized;

            // Zniszcz laser po czasie laserLifetime
            Destroy(activeLaser, laserLifetime);
        }
    }
}

