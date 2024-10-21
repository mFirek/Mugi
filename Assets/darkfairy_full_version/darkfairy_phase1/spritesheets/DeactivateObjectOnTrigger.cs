using UnityEngine;
using UnityEngine.UI; // Dodaj referencjê do UnityEngine.UI, aby korzystaæ z elementów UI

public class DeactivateObjectOnTrigger : MonoBehaviour
{
    public GameObject objectToDeactivate; // Referencja do obiektu klucza
    public GameObject keyUI; // Referencja do UI, które informuje o zebraniu klucza
    private bool isCollected = false;

    AudioManager audioManager;

    private void Start()
    {
        // Upewnij siê, ¿e UI jest ukryte na starcie gry
        keyUI.SetActive(false);
        audioManager = AudioManager.GetInstance();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCollected)
        {
            objectToDeactivate.SetActive(false); // dezaktywacja klucza
            AudioManager.GetInstance().PlaySFX(audioManager.PickUp);
            keyUI.SetActive(true); // Poka¿ UI informuj¹ce o zebraniu klucza
            isCollected = true; // oznaczenie klucza jako podniesionego
        }
    }


    // Metoda do ponownej aktywacji klucza
    public void RespawnKey()
    {
        objectToDeactivate.SetActive(true); // aktywacja klucza
        keyUI.SetActive(false); // Ukryj UI, poniewa¿ klucz znowu jest na scenie
        isCollected = false; // reset stanu klucza
    }
}
