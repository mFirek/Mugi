//using UnityEngine;
//using UnityEngine.SceneManagement;
//using System.Collections;

//public class LevelManager : MonoBehaviour
//{



//    public string nextLevelName; // Nazwa kolejnej sceny
//    public string playerTag = "Player"; // Tag gracza
//    public Transform absorptionPoint; // Punkt wch³aniania w œrodku portalu
//    public float absorptionDuration = 2f; // Czas trwania animacji wch³aniania
//    public float smoothTime = 0.3f; // Czas wyg³adzania ruchu

//    private bool isAbsorbing = false;
//    private Vector3 velocity = Vector3.zero; // Zmienna dla SmoothDamp

//    AudioManager audioManager;

//    void Start()
//    {
//        audioManager = AudioManager.GetInstance();
//    }   


//    void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag(playerTag) && !isAbsorbing)
//        {
//            isAbsorbing = true; // Zabezpieczenie, aby efekt nie aktywowa³ siê wielokrotnie
//            StartCoroutine(AbsorbAndLoadLevel(other.transform));
//            AudioManager.GetInstance().PlaySFX(audioManager.Portal); // Odtwórz dŸwiêk portalu z g³oœnoœci¹ 1.5   
//        }
//    }

//    IEnumerator AbsorbAndLoadLevel(Transform player)
//    {
//        // Zablokuj ruch gracza
//        CharacterController2D playerController = player.GetComponent<CharacterController2D>();
//        if (playerController != null)
//        {
//            playerController.enabled = false;
//        }

//        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
//        if (playerRigidbody != null)
//        {
//            playerRigidbody.velocity = Vector2.zero; // Zatrzymaj gracza
//            playerRigidbody.isKinematic = true; // Wy³¹cz fizykê gracza na czas wch³aniania
//        }

//        float elapsedTime = 0f;
//        Vector3 originalScale = player.localScale;
//        Vector3 targetScale = Vector3.zero; // Zmniejsz do zera

//        while (elapsedTime < absorptionDuration)
//        {
//            // P³ynne przemieszczanie gracza w stronê punktu wch³aniania z u¿yciem SmoothDamp
//            player.position = Vector3.SmoothDamp(player.position, absorptionPoint.position, ref velocity, smoothTime);

//            // P³ynna zmiana skali
//            player.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / absorptionDuration);

//            // Obracaj gracza, aby daæ efekt wch³aniania
//            player.Rotate(Vector3.forward, 360 * Time.deltaTime); // Mo¿esz dostosowaæ szybkoœæ rotacji

//            elapsedTime += Time.deltaTime;
//            yield return null;
//        }

//        // Upewnij siê, ¿e gracz zosta³ wch³oniêty ca³kowicie
//        player.localScale = targetScale;

//        // Przywróæ fizykê, jeœli potrzebne
//        if (playerRigidbody != null)
//        {
//            playerRigidbody.isKinematic = false;
//        }

//        // Za³aduj kolejny poziom
//        SceneManager.LoadScene(nextLevelName);
//        GlobalDeathCounter.SaveGlobalDeathCount();
//    }
//}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public string nextLevelName;
    public string playerTag = "Player";
    public Transform absorptionPoint;
    public float absorptionDuration = 2f;
    public float smoothTime = 0.3f;
    public Image fadePanel; // Panel u¿ywany tylko do efektu fade-out
    public float fadeDuration = 1f;

    private bool isAbsorbing = false;
    private Vector3 velocity = Vector3.zero;

    AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.GetInstance();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && !isAbsorbing)
        {
            isAbsorbing = true;
            StartCoroutine(AbsorbAndFadeOut(other.transform));
            AudioManager.GetInstance().PlaySFX(audioManager.Portal);
        }
    }

    IEnumerator AbsorbAndFadeOut(Transform player)
    {
        CharacterController2D playerController = player.GetComponent<CharacterController2D>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.isKinematic = true;
        }

        float elapsedTime = 0f;
        Vector3 originalScale = player.localScale;
        Vector3 targetScale = Vector3.zero;

        while (elapsedTime < absorptionDuration)
        {
            player.position = Vector3.SmoothDamp(player.position, absorptionPoint.position, ref velocity, smoothTime);
            player.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / absorptionDuration);
            player.Rotate(Vector3.forward, 360 * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.localScale = targetScale;

        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false;
        }

        yield return StartCoroutine(FadeOut());

        SceneManager.LoadScene(nextLevelName);
        GlobalDeathCounter.SaveGlobalDeathCount();
    }

    IEnumerator FadeOut()
    {
        float fadeElapsed = 0f;
        while (fadeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, fadeElapsed / fadeDuration);
            fadePanel.color = new Color(0, 119, 255, alpha);
            fadeElapsed += Time.deltaTime;
            yield return null;
        }
        fadePanel.color = new Color(0, 119, 255, 1);
    }
}



