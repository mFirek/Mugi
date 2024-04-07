using UnityEngine;

public class Enemies_Death : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D[] colliders;

    // Przesuniêcie w dó³ podczas animacji
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

                // Przesuñ obiekt trochê w dó³ podczas animacji
                transform.position += deathAnimationOffset;

                // Zniszcz obiekt po zakoñczeniu animacji
                Destroy(gameObject, GetAnimationLength(animator, "Death"));
            }
            else
            {
                // Jeœli nie znaleziono animatora, zniszcz obiekt bez odtwarzania animacji
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
