//using UnityEngine;

//public class Enemies_Death : MonoBehaviour
//{
//    Rigidbody2D rb;
//    Collider2D[] colliders;

//    // Przesuniêcie w dó³ podczas animacji
//    public Vector3 deathAnimationOffset = new Vector3(0f, -0.5f, 0f);

//    void Start()
//    {
//        rb = GetComponentInChildren<Rigidbody2D>();
//        colliders = GetComponentsInChildren<Collider2D>();
//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Kula"))
//        {
//            Animator animator = GetComponentInChildren<Animator>();

//            if (animator != null)
//            {
//                // Wy³¹cz fizykê obiektu
//                if (rb != null)
//                {
//                    rb.simulated = false;
//                }

//                // Wy³¹cz collidery obiektu
//                foreach (Collider2D collider in colliders)
//                {
//                    collider.enabled = false;
//                }

//                // Uruchom animacjê œmierci
//                animator.SetTrigger("Death");

//                // Przesuñ obiekt trochê w dó³ podczas animacji
//                transform.position += deathAnimationOffset;

//                // Zniszcz obiekt po zakoñczeniu animacji
//                Destroy(gameObject, GetAnimationLength(animator, "Death"));
//            }
//            else
//            {
//                // Jeœli nie znaleziono animatora, zniszcz obiekt bez odtwarzania animacji
//                Destroy(gameObject);
//            }
//        }
//    }

//    float GetAnimationLength(Animator animator, string triggerName)
//    {
//        if (animator != null)
//        {
//            // Pobierz czas trwania animacji
//            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
//            foreach (AnimationClip clip in clips)
//            {
//                if (clip.name == triggerName)
//                {
//                    return clip.length;
//                }
//            }
//        }
//        return 0f;
//    }
//}
using UnityEngine;

public class Enemies_Death : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D[] colliders;

    // Czas migania postaci po trafieniu pociskiem
    public float blinkDuration = 0.5f;

    // Flaga informuj¹ca, czy postaæ jest w trakcie miganie
    private bool isBlinking = false;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Kula"))
        {
            // Zniszcz pocisk
            Destroy(collision.gameObject);

            Animator animator = GetComponentInChildren<Animator>();

            if (animator != null && HasDeathAnimation(animator))
            {
                // Wy³¹cz fizykê obiektu
                if (rb != null)
                {
                    rb.simulated = false;
                }

                // Wy³¹cz collidery obiektu
                foreach (Collider2D collider in colliders)
                {
                    collider.enabled = false;
                }

                // Uruchom animacjê œmierci
                animator.SetTrigger("Death");

                // Zniszcz obiekt po zakoñczeniu animacji
                Destroy(gameObject, GetAnimationLength(animator, "Death"));
            }
            else
            {
                // Jeœli nie znaleziono animatora lub animacji "Death", zacznij migotanie i zniszcz obiekt po miganiu
                if (!isBlinking)
                {
                    isBlinking = true;
                    StartCoroutine(BlinkAndDestroy());
                }
            }
        }
    }

    bool HasDeathAnimation(Animator animator)
    {
        if (animator != null)
        {
            // SprawdŸ, czy animacja "Death" istnieje
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == "Death")
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Migotanie postaci na bia³o i zniszczenie jej po miganiu
    private System.Collections.IEnumerator BlinkAndDestroy()
    {
        // Zapêtlenie migania przez okreœlony czas
        float timer = 0f;
        while (timer < blinkDuration)
        {
            // Zmieñ kolor wszystkich renderów na bia³y
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = Color.white;
            }

            // Czekaj krótk¹ chwilê
            yield return new WaitForSeconds(0.1f);

            // Zmieñ kolor wszystkich renderów na pierwotny
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = Color.white;
            }

            // Czekaj krótk¹ chwilê
            yield return new WaitForSeconds(0.1f);

            // Aktualizuj czas
            timer += 0.2f;
        }

        // Wy³¹cz postaæ po zakoñczeniu migania
        gameObject.SetActive(false);
    }

    float GetAnimationLength(Animator animator, string triggerName)
    {
        if (animator != null)
        {
            // Pobierz czas trwania animacji
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == triggerName)
                {
                    return clip.length;
                }
            }
        }
        return 0f;
    }
}



