using UnityEngine;

public class DeactivateIfMissing : MonoBehaviour
{
    public GameObject keyObject; // klucz, który ma znikn¹æ przed aktywacj¹ drzwi
    public GameObject doorObject; // drzwi do dezaktywacji

    private bool keyIsMissing = false; // flaga okreœlaj¹ca, czy klucz zosta³ znikniêty

    private void Update()
    {
        // SprawdŸ, czy klucz znikn¹³
        if (!keyIsMissing && (keyObject == null || !keyObject.activeInHierarchy))
        {
            keyIsMissing = true; // ustaw flagê na true, gdy klucz zniknie
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // sprawdzenie, czy obiekt jest graczem
        {
            // Jeœli klucz znikn¹³ i drzwi s¹ obecne, dezaktywuj drzwi
            if (keyIsMissing && doorObject != null)
            {
                doorObject.SetActive(false);
            }
        }
    }
}