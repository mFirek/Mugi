using UnityEngine;

public class DeactivateIfMissing : MonoBehaviour
{
    public GameObject keyObject; // klucz, kt�ry ma znikn�� przed aktywacj� drzwi
    public GameObject doorObject; // drzwi do dezaktywacji

    private bool keyIsMissing = false; // flaga okre�laj�ca, czy klucz zosta� znikni�ty

    private void Update()
    {
        // Sprawd�, czy klucz znikn��
        if (!keyIsMissing && (keyObject == null || !keyObject.activeInHierarchy))
        {
            keyIsMissing = true; // ustaw flag� na true, gdy klucz zniknie
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // sprawdzenie, czy obiekt jest graczem
        {
            // Je�li klucz znikn�� i drzwi s� obecne, dezaktywuj drzwi
            if (keyIsMissing && doorObject != null)
            {
                doorObject.SetActive(false);
            }
        }
    }
}