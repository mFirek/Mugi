using UnityEngine;

public class BossSummonBats : MonoBehaviour
{
    public GameObject batPrefab; // Prefab nietoperza
    public Transform summonPoint; // Punkt przywo�ania nietoperzy

    // Metoda, kt�ra zostanie wywo�ana przez animacj�
    public void SummonBats()
    {
        Instantiate(batPrefab, summonPoint.position, summonPoint.rotation);
    }
}
