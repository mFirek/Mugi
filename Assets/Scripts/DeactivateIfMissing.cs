using UnityEngine;

public class DeactivateIfMissing : MonoBehaviour
{
    public GameObject keyObject; // klucz, który ma byæ nieaktywny przed aktywacj¹ drzwi
    public GameObject doorObject; // drzwi do dezaktywacji

    private bool keyIsInactive = false; // flaga okreœlaj¹ca, czy klucz jest nieaktywny

    private void Update()
    {
        // SprawdŸ, czy klucz istnieje i jest nieaktywny
        if (keyObject != null && !keyObject.activeInHierarchy)
        {
            keyIsInactive = true; // ustaw flagê na true, gdy klucz jest nieaktywny
        }
        else
        {
            keyIsInactive = false; // ustaw na false, gdy klucz jest aktywny
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // sprawdzenie, czy obiekt jest graczem
        {
            // Debug.Log - sprawdzamy stan klucza i drzwi
            Debug.Log($"Key is inactive: {keyIsInactive}");
            Debug.Log($"Key active in hierarchy: {keyObject?.activeInHierarchy}");

            // Jeœli klucz jest nieaktywny i drzwi istniej¹, dezaktywuj drzwi
            if (keyIsInactive && doorObject != null)
            {
                doorObject.SetActive(false);
                Debug.Log("Door deactivated");
            }
        }
    }
}

