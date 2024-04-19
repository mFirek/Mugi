using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuringOffColliders : MonoBehaviour
{
    public float newColliderSize = 0.5f; // Nowy rozmiar Box Collider 2D
    public bool disablePolygonCollider = true; // Czy wy³¹czyæ Polygon Collider 2D?

    void Start()
    {
        // Pobierz komponent Box Collider 2D
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider != null)
        {
            // Zmieñ rozmiar Box Collider 2D zgodnie z ustawieniami
            boxCollider.size *= newColliderSize;
        }
        else
        {
            Debug.LogWarning("Nie znaleziono Box Collider 2D na obiekcie.");
        }

        if (disablePolygonCollider)
        {
            // Wy³¹cz Polygon Collider 2D
            PolygonCollider2D polyCollider = GetComponent<PolygonCollider2D>();
            if (polyCollider != null)
            {
                polyCollider.enabled = false;
            }
            else
            {
                Debug.LogWarning("Nie znaleziono Polygon Collider 2D na obiekcie.");
            }
        }
    }
}