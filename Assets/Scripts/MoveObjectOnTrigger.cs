using System.Collections;
using UnityEngine;

public class MoveObjectOnTrigger : MonoBehaviour
{
    public GameObject objectToMove; // Obiekt, kt�ry ma by� przesuni�ty
    public float moveDistance = -5f; // Odleg�o�� przesuni�cia po osi X (ujemna dla lewego kierunku)
    public float moveSpeed = 2f; // Pr�dko�� przesuni�cia obiektu
    public float returnDelay = 1f; // Czas oczekiwania przed powrotem do pozycji pocz�tkowej
    private bool isMoving = false; // Flaga informuj�ca, czy obiekt si� porusza
    private Vector3 startPosition; // Pozycja pocz�tkowa obiektu
    private Vector3 targetPosition; // Pozycja docelowa dla obiektu
    private SpriteRenderer[] spriteRenderers; // Tablica SpriteRenderer�w

    void Start()
    {
        // Zapisujemy pocz�tkow� pozycj� obiektu
        startPosition = objectToMove.transform.position;

        // Ustawiamy pozycj� docelow� (aktualna pozycja + przesuni�cie po osi X)
        targetPosition = startPosition + new Vector3(moveDistance, 0, 0);

        // Pobieramy wszystkie komponenty SpriteRenderer na obiekcie i jego dzieciach
        spriteRenderers = objectToMove.GetComponentsInChildren<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isMoving)
        {
            // Gdy gracz wejdzie w trigger, rozpoczynamy przesuwanie obiektu
            StartCoroutine(MoveObjectAndRespawn());
        }
    }

    IEnumerator MoveObjectAndRespawn()
    {
        isMoving = true;

        // Przesuwamy obiekt do pozycji docelowej
        while (Vector3.Distance(objectToMove.transform.position, targetPosition) > 0.1f)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Ustawiamy dok�adnie pozycj� docelow�, aby unikn�� niedok�adno�ci
        objectToMove.transform.position = targetPosition;

        // Czekamy przez ustalony czas przed powrotem (np. 1 sekunda)
        yield return new WaitForSeconds(returnDelay);

        // Ukrywamy obiekt (wy��czamy renderowanie wszystkich SpriteRenderer�w)
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = false;
        }

        // Natychmiast przenosimy obiekt do jego pozycji pocz�tkowej
        objectToMove.transform.position = startPosition;

        // Ponownie w��czamy renderowanie obiektu (pokazujemy go)
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = true;
        }

        isMoving = false; // Resetujemy flag�, by obiekt m�g� zn�w by� przesuni�ty
    }
}
