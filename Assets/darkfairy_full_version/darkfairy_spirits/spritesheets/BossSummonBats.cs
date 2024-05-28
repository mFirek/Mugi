using UnityEngine;

public class BossSummonBats : MonoBehaviour
{
    public GameObject batPrefab; // Prefab nietoperza
    public Transform summonPoint; // Punkt przywo³ania nietoperzy

    // Metoda, która zostanie wywo³ana przez animacjê
    public void SummonBats()
    {
        Instantiate(batPrefab, summonPoint.position, summonPoint.rotation);
    }
}
