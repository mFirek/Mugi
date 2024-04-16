using UnityEngine;
using System.Collections;

public class SpikeGame2 : MonoBehaviour
{
    public GameObject spikePrefab; // Prefabrykat kolca
    public Transform[] spikePositions; // Pozycje, w kt�rych mog� pojawi� si� kolce
    public float spikeCycleTime = 2f; // Czas cyklu pojawiania si� i znikania kolc�w (w sekundach)

    private bool spikesVisible = true; // Flaga okre�laj�ca, czy kolce s� widoczne

    private Coroutine spikeCoroutine; // Referencja do coroutine

    void Start()
    {
        // Uruchomienie coroutine i zachowanie referencji do niego
        spikeCoroutine = StartCoroutine(SpikeCycle());
    }

    void OnDestroy()
    {
        // Zatrzymanie coroutine, gdy obiekt jest niszczony
        if (spikeCoroutine != null)
        {
            StopCoroutine(spikeCoroutine);
        }
    }

    IEnumerator SpikeCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(spikeCycleTime);
            ToggleSpikes();
        }
    }

    void ToggleSpikes()
    {
        spikesVisible = !spikesVisible;

        if (spikesVisible)
        {
            ShowSpikes();
        }
        else
        {
            HideSpikes();
        }
    }

    void ShowSpikes()
    {
        foreach (Transform position in spikePositions)
        {
            // Sprawdzenie, czy prefabrykat kolca nie zosta� zniszczony
            if (spikePrefab != null)
            {
                GameObject spikeInstance = Instantiate(spikePrefab, position.position, Quaternion.Euler(0, 0, 90));
            }
        }
    }

    void HideSpikes()
    {
        GameObject[] spikes = GameObject.FindGameObjectsWithTag("Spike");
        foreach (GameObject spike in spikes)
        {
            Destroy(spike);
        }
    }
}
