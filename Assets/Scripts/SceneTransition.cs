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
            // Ustawienie koloru zielonego przy pe³nej przezroczystoœci (zielony = 0,255,0)
            fadePanel.color = new Color(0f / 255f, 255f / 255f, 0f / 255f, 1); // Pe³ne zaciemnienie zielone
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
            fadePanel.color = new Color(0f / 255f, 255f / 255f, 0f / 255f, alpha); // Utrzymanie koloru zielonego, zmiana przezroczystoœci
            fadeElapsed += Time.deltaTime;
            yield return null;
        }
        fadePanel.color = new Color(0f / 255f, 255f / 255f, 0f / 255f, 0); // Ustawienie ca³kowitej przezroczystoœci
    }
}
