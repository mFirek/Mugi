using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 1.5f; // Wyd³u¿ony czas trwania dla bardziej p³ynnego efektu

    void Start()
    {
        if (fadePanel != null)
        {
            fadePanel.color = new Color(0, 81, 43, 0); // Ustaw pe³ne zaciemnienie
            StartCoroutine(FadeIn()); // Rozpocznij fade-in przy starcie
        }
    }

    public IEnumerator FadeIn()
    {
        float fadeElapsed = 0f;
        while (fadeElapsed < fadeDuration)
        {
            // U¿ycie SmoothStep dla p³ynniejszej zmiany przezroczystoœci
            float alpha = Mathf.SmoothStep(1, 0, fadeElapsed / fadeDuration);
            fadePanel.color = new Color(0, 81, 43, alpha);
            fadeElapsed += Time.deltaTime;
            yield return null;
        }
        fadePanel.color = new Color(0, 81, 43, 0);
    }
}
