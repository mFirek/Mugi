using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PortalTriggerEffect : MonoBehaviour
{
    public Volume volume; // Przypisz Volume z efektem kamery w inspektorze
    private LensDistortion lensDistortion;
    private bool effectActive = false;

    void Start()
    {
        if (volume.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            lensDistortion.intensity.value = 0f; // Efekt wy³¹czony na pocz¹tku
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Upewnij siê, ¿e gracz wchodzi w trigger
        {
            StartCoroutine(ActivateEffect());
        }
    }

    private IEnumerator ActivateEffect()
    {
        yield return new WaitForSeconds(2f);
        effectActive = true;
        float elapsed = 0f;
        float duration = 1f;

     
        // Wy³¹cz efekt dystorsji
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            lensDistortion.intensity.value = Mathf.Lerp(0, 1f, elapsed / duration);
            yield return null;
        }

        lensDistortion.intensity.value = 0f;
        effectActive = false;
    }
}
