using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObjectOnTrigger : MonoBehaviour
{
    public GameObject objectToDeactivate;
    private bool isCollected = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCollected) // sprawdzenie, czy obiekt jest graczem i klucz nie zosta³ wczeœniej podniesiony
        {
            objectToDeactivate.SetActive(false); // dezaktywacja klucza
            isCollected = true; // oznaczenie klucza jako podniesionego
        }
    }

    // Metoda do ponownej aktywacji klucza
    public void RespawnKey()
    {
        objectToDeactivate.SetActive(true); // aktywacja klucza
        isCollected = false; // reset stanu klucza
    }
}
