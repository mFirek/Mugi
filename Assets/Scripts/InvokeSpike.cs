using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvokeSpike : MonoBehaviour
{
    public float blinkInterval = 0.5f; // Czas miêdzy kolejnymi migniêciami
    public float respawnDelay = 1f; // OpóŸnienie przed respawnem
    public Color blinkColor = Color.white; // Kolor migania
    private GameObject player;
    private bool isPlayerInside = false; // Flaga okreœlaj¹ca, czy gracz jest wewn¹trz obszaru
    private Renderer playerRenderer; // Komponent Renderer gracza
    private Color originalColor; // Oryginalny kolor gracza

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRenderer = player.GetComponent<Renderer>(); // Pobierz komponent Renderer gracza
        originalColor = playerRenderer.material.color; // Zapisz oryginalny kolor gracza
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            StartCoroutine(BlinkPlayerRoutine());
            Invoke("RespawnPlayer", respawnDelay); // Wywo³aj funkcjê RespawnPlayer po okreœlonym opóŸnieniu
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            StopAllCoroutines(); // Zatrzymaj wszystkie korutyny
            playerRenderer.material.color = originalColor; // Przywróæ oryginalny kolor gracza
        }
    }

    IEnumerator BlinkPlayerRoutine()
    {
        while (true)
        {
            // Zmieñ kolor gracza na wybrany kolor migania
            playerRenderer.material.color = blinkColor;
            yield return new WaitForSeconds(blinkInterval);

            // Przywróæ oryginalny kolor gracza
            playerRenderer.material.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private void RespawnPlayer()
    {
        if (isPlayerInside)
        {
            // Przenieœ gracza do punktu respawnu
            Vector2 respawnPoint = GameManager.Instance.GetSpawnPoint();
            player.transform.position = respawnPoint;
            StopAllCoroutines(); // Zatrzymaj wszystkie korutyny (miganie)
            playerRenderer.material.color = originalColor; // Przywróæ oryginalny kolor gracza
        }
    }
}
