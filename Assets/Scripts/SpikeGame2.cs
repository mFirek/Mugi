using UnityEngine;
using System.Collections;

public class SpikeGame2 : MonoBehaviour
{
    public GameObject spikePrefab; // Prefabrykat kolca
    public Transform[] spikePositions; // Pozycje, w których mog¹ pojawiæ siê kolce
    public float spikeCycleTime = 2f; // Czas cyklu pojawiania siê i znikania kolców (w sekundach)

    private bool spikesVisible = true; // Flaga okreœlaj¹ca, czy kolce s¹ widoczne

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
            // Sprawdzenie, czy prefabrykat kolca nie zosta³ zniszczony
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
