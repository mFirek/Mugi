using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class SceneLoadEffect : MonoBehaviour
{
    public Volume volume;
    private LensDistortion lensDistortion;
    private bool effectActive = false;

    void Start()
    {
        // Pobierz efekt dystorsji z Volume
        if (volume.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            lensDistortion.intensity.value = 0f;
            StartCoroutine(ActivateEffect());
        }
    }

    private IEnumerator ActivateEffect()
    {
        effectActive = true;
        float elapsed = 0f;
        float duration = 1f;

        //// Animacja w³¹czania efektu dystorsji
        //while (elapsed < duration)
        //{
        //    elapsed += Time.deltaTime;
        //    lensDistortion.intensity.value = Mathf.Lerp(1f, -1f, elapsed / duration);
        //    yield return null;
        //}

        //yield return new WaitForSeconds(1f); // Efekt pozostaje aktywny przez 1 sekundê

        // Animacja wy³¹czania efektu dystorsji
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            lensDistortion.intensity.value = Mathf.Lerp(1f, 0, elapsed / duration);
            yield return null;
        }

        lensDistortion.intensity.value = 0f;
        effectActive = false;
    }
}
