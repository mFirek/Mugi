using System.Collections;
using UnityEngine;

public class InvokeSpike : MonoBehaviour
{
    public float blinkInterval = 0.5f; // Czas miêdzy kolejnymi migniêciami
    public float respawnDelay = 1f;    // OpóŸnienie przed respawnem
    public Color blinkColor = Color.red; // Kolor migania
    private GameObject player;
    private bool isPlayerInside = false; // Flaga okreœlaj¹ca, czy gracz jest wewn¹trz obszaru
    private SpriteRenderer playerRenderer; // Komponent SpriteRenderer gracza
    private Color originalColor; // Oryginalny kolor gracza
    private bool isDead = false;  // Flaga zapobiegaj¹ca wielokrotnemu zliczaniu zgonów

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerRenderer = player.GetComponent<SpriteRenderer>(); // Pobierz SpriteRenderer
            if (playerRenderer != null)
            {
                originalColor = playerRenderer.color; // Zapisz oryginalny kolor gracza
            }
            else
            {
                Debug.LogError("Brak komponentu SpriteRenderer na obiekcie gracza!");
            }
        }
        else
        {
            Debug.LogError("Nie znaleziono obiektu gracza z tagiem 'Player'!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDead)
        {
            isDead = true; // Zabezpieczenie przed wielokrotnym zgonem
            isPlayerInside = true;
            StartCoroutine(BlinkPlayerRoutine());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Resetuj platformê po wyjœciu gracza
            ResetPlatformState();
        }
    }

    IEnumerator BlinkPlayerRoutine()
    {
        float elapsed = 0f;
        while (elapsed < respawnDelay)
        {
            // Miganie gracza
            if (playerRenderer != null)
            {
                playerRenderer.color = blinkColor; // Zmieñ kolor na kolor migania
                yield return new WaitForSeconds(blinkInterval);
                playerRenderer.color = originalColor; // Przywróæ oryginalny kolor
                yield return new WaitForSeconds(blinkInterval);
            }

            elapsed += 2 * blinkInterval; // Zlicz czas migania
        }

        // Po zakoñczeniu migania wykonaj respawn
        if (isPlayerInside)
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.PlayerDied();
        }

        if (GameManager.Instance != null)
        {
            // Przenieœ gracza do punktu respawnu
            Vector2 respawnPoint = GameManager.Instance.GetSpawnPoint();
            player.transform.position = respawnPoint;
        }
        else
        {
            Debug.LogError("GameManager.Instance jest null!");
        }

        // Przywróæ kolor gracza
        if (playerRenderer != null)
        {
            playerRenderer.color = originalColor;
        }

        // Resetuj flagê 'isDead' od razu po respawnie
        isDead = false;
    }

    private void ResetPlatformState()
    {
        // Zatrzymaj wszystkie korutyny po opuszczeniu platformy
        StopAllCoroutines();

        // Przywróæ oryginalny kolor gracza
        if (playerRenderer != null)
        {
            playerRenderer.color = originalColor;
        }

        // Resetuj flagê 'isDead', aby platforma mog³a naliczaæ œmieræ po ponownym wejœciu
        isDead = false;

        // Zresetuj stan "gracza wewn¹trz" platformy
        isPlayerInside = false;
    }
}
