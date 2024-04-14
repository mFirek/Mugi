//using UnityEngine;

//public class Enemies_Death : MonoBehaviour
//{
//    Rigidbody2D rb;
//    Collider2D[] colliders;

//    // Przesuni�cie w d� podczas animacji
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
//                // Wy��cz fizyk� obiektu
//                if (rb != null)
//                {
//                    rb.simulated = false;
//                }

//                // Wy��cz collidery obiektu
//                foreach (Collider2D collider in colliders)
//                {
//                    collider.enabled = false;
//                }

//                // Uruchom animacj� �mierci
//                animator.SetTrigger("Death");

//                // Przesu� obiekt troch� w d� podczas animacji
//                transform.position += deathAnimationOffset;

//                // Zniszcz obiekt po zako�czeniu animacji
//                Destroy(gameObject, GetAnimationLength(animator, "Death"));
//            }
//            else
//            {
//                // Je�li nie znaleziono animatora, zniszcz obiekt bez odtwarzania animacji
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

    // Flaga informuj�ca, czy posta� jest w trakcie miganie
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
                // Wy��cz fizyk� obiektu
                if (rb != null)
                {
                    rb.simulated = false;
                }

                // Wy��cz collidery obiektu
                foreach (Collider2D collider in colliders)
                {
                    collider.enabled = false;
                }

                // Uruchom animacj� �mierci
                animator.SetTrigger("Death");

                // Zniszcz obiekt po zako�czeniu animacji
                Destroy(gameObject, GetAnimationLength(animator, "Death"));
            }
            else
            {
                // Je�li nie znaleziono animatora lub animacji "Death", zacznij migotanie i zniszcz obiekt po miganiu
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
            // Sprawd�, czy animacja "Death" istnieje
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

    // Migotanie postaci na bia�o i zniszczenie jej po miganiu
    private System.Collections.IEnumerator BlinkAndDestroy()
    {
        // Zap�tlenie migania przez okre�lony czas
        float timer = 0f;
        while (timer < blinkDuration)
        {
            // Zmie� kolor wszystkich render�w na bia�y
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = Color.white;
            }

            // Czekaj kr�tk� chwil�
            yield return new WaitForSeconds(0.1f);

            // Zmie� kolor wszystkich render�w na pierwotny
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = Color.white;
            }

            // Czekaj kr�tk� chwil�
            yield return new WaitForSeconds(0.1f);

            // Aktualizuj czas
            timer += 0.2f;
        }

        // Wy��cz posta� po zako�czeniu migania
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



