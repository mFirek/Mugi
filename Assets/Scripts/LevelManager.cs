using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{



    public string nextLevelName; // Nazwa kolejnej sceny
    public string playerTag = "Player"; // Tag gracza
    public Transform absorptionPoint; // Punkt wch�aniania w �rodku portalu
    public float absorptionDuration = 2f; // Czas trwania animacji wch�aniania
    public float smoothTime = 0.3f; // Czas wyg�adzania ruchu

    private bool isAbsorbing = false;
    private Vector3 velocity = Vector3.zero; // Zmienna dla SmoothDamp

    AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.GetInstance();
    }   


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && !isAbsorbing)
        {
            isAbsorbing = true; // Zabezpieczenie, aby efekt nie aktywowa� si� wielokrotnie
            StartCoroutine(AbsorbAndLoadLevel(other.transform));
            AudioManager.GetInstance().PlaySFX(audioManager.Portal); // Odtw�rz d�wi�k portalu z g�o�no�ci� 1.5   
        }
    }

    IEnumerator AbsorbAndLoadLevel(Transform player)
    {
        // Zablokuj ruch gracza
        CharacterController2D playerController = player.GetComponent<CharacterController2D>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector2.zero; // Zatrzymaj gracza
            playerRigidbody.isKinematic = true; // Wy��cz fizyk� gracza na czas wch�aniania
        }

        float elapsedTime = 0f;
        Vector3 originalScale = player.localScale;
        Vector3 targetScale = Vector3.zero; // Zmniejsz do zera

        while (elapsedTime < absorptionDuration)
        {
            // P�ynne przemieszczanie gracza w stron� punktu wch�aniania z u�yciem SmoothDamp
            player.position = Vector3.SmoothDamp(player.position, absorptionPoint.position, ref velocity, smoothTime);

            // P�ynna zmiana skali
            player.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / absorptionDuration);

            // Obracaj gracza, aby da� efekt wch�aniania
            player.Rotate(Vector3.forward, 360 * Time.deltaTime); // Mo�esz dostosowa� szybko�� rotacji

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Upewnij si�, �e gracz zosta� wch�oni�ty ca�kowicie
        player.localScale = targetScale;

        // Przywr�� fizyk�, je�li potrzebne
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false;
        }

        // Za�aduj kolejny poziom
        SceneManager.LoadScene(nextLevelName);
        GlobalDeathCounter.SaveGlobalDeathCount();
    }
}
