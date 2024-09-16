using System.Collections;
using UnityEngine;

public class MoveObjectOnTrigger : MonoBehaviour
{
    public GameObject objectToMove; // Obiekt, który ma byæ przesuniêty
    public float moveDistance = -5f; // Odleg³oœæ przesuniêcia po osi X (ujemna dla lewego kierunku)
    public float moveSpeed = 2f; // Prêdkoœæ przesuniêcia obiektu
    public float returnDelay = 1f; // Czas oczekiwania przed powrotem do pozycji pocz¹tkowej
    private bool isMoving = false; // Flaga informuj¹ca, czy obiekt siê porusza
    private Vector3 startPosition; // Pozycja pocz¹tkowa obiektu
    private Vector3 targetPosition; // Pozycja docelowa dla obiektu
    private SpriteRenderer[] spriteRenderers; // Tablica SpriteRendererów

    void Start()
    {
        // Zapisujemy pocz¹tkow¹ pozycjê obiektu
        startPosition = objectToMove.transform.position;

        // Ustawiamy pozycjê docelow¹ (aktualna pozycja + przesuniêcie po osi X)
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

        // Ustawiamy dok³adnie pozycjê docelow¹, aby unikn¹æ niedok³adnoœci
        objectToMove.transform.position = targetPosition;

        // Czekamy przez ustalony czas przed powrotem (np. 1 sekunda)
        yield return new WaitForSeconds(returnDelay);

        // Ukrywamy obiekt (wy³¹czamy renderowanie wszystkich SpriteRendererów)
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = false;
        }

        // Natychmiast przenosimy obiekt do jego pozycji pocz¹tkowej
        objectToMove.transform.position = startPosition;

        // Ponownie w³¹czamy renderowanie obiektu (pokazujemy go)
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = true;
        }

        isMoving = false; // Resetujemy flagê, by obiekt móg³ znów byæ przesuniêty
    }
}
