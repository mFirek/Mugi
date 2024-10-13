using UnityEngine;
using System.Collections;

public class SpikeGame : MonoBehaviour
{
    public GameObject spikePrefab; // Prefabricated spike
    public Transform[] spikePositions; // Positions in which spikes may appear
    public float spikeCycleTime = 2f; // Cycle time of appearance and disappearance of spikes (in seconds)

    private bool spikesVisible = true; // Flag to determine whether spikes are visible

    private Coroutine spikeCoroutine; // Reference to coroutine

    void Start()
    {
        // Running a coroutine and keeping a reference to it
        spikeCoroutine = StartCoroutine(SpikeCycle());
    }

    void OnDestroy()
    {
        // Stop coroutine when object is destroyed
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
            // Check that the prefabricated spike has not been destroyed
            if (spikePrefab != null)
            {
                GameObject spikeInstance = Instantiate(spikePrefab, position.position, Quaternion.Euler(0, 0, -90));
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
