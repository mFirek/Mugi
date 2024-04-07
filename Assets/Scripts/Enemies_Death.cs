using UnityEngine;

public class Enemies_Death : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D[] colliders;

    // Przesuni�cie w d� podczas animacji
    public Vector3 deathAnimationOffset = new Vector3(0f, -0.5f, 0f);

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        colliders = GetComponentsInChildren<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Kula"))
        {
            Animator animator = GetComponentInChildren<Animator>();

            if (animator != null)
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

                // Przesu� obiekt troch� w d� podczas animacji
                transform.position += deathAnimationOffset;

                // Zniszcz obiekt po zako�czeniu animacji
                Destroy(gameObject, GetAnimationLength(animator, "Death"));
            }
            else
            {
                // Je�li nie znaleziono animatora, zniszcz obiekt bez odtwarzania animacji
                Destroy(gameObject);
            }
        }
    }

    float GetAnimationLength(Animator animator, string triggerName)
    {
        if (animator != null)
        {
            // Pobierz czas trwania animacji
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
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
