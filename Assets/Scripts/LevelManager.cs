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
    public Image fadePanel;
    public float fadeDuration = 1f;

    private bool isAbsorbing = false;
    private Vector3 velocity = Vector3.zero;

    private AudioManager audioManager;
    private PlayerLevelAnalytics playerLevelAnalytics;

    void Start()
    {
        audioManager = AudioManager.GetInstance();
        playerLevelAnalytics = GetComponent<PlayerLevelAnalytics>();

        // Sprawdzenie czy skrypt PlayerLevelAnalytics zosta³ znaleziony
        if (playerLevelAnalytics == null)
        {
            Debug.LogError("PlayerLevelAnalytics nie zosta³ znaleziony na tym obiekcie!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && !isAbsorbing)
        {
            isAbsorbing = true;
            StartCoroutine(AbsorbAndFadeOut(other.transform));
            audioManager.PlaySFX(audioManager.Portal);
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

        // Uruchom efekt fade-out
        yield return StartCoroutine(FadeOut());

        // Sprawdzenie wywo³ania zdarzenia koñca poziomu
       
        if (playerLevelAnalytics != null)
        {
            playerLevelAnalytics.SendLevelEndEvent();
            
        }
        else
        {
            Debug.LogError("Brak przypisanej referencji do PlayerLevelAnalytics");
        }

        // Przejœcie do kolejnego poziomu
        SceneManager.LoadScene(nextLevelName);
    }

    IEnumerator FadeOut()
    {
        float fadeElapsed = 0f;
        Color originalColor = fadePanel.color;

        while (fadeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, fadeElapsed / fadeDuration);
            fadePanel.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            fadeElapsed += Time.deltaTime;
            yield return null;
        }

        fadePanel.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }
}
