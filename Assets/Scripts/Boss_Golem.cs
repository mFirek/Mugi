using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Golem : MonoBehaviour
{
    public Transform player;
    public bool isFlipped = false;

    private bool isGlowing = false; // Zmienna okre�laj�ca, czy boss �wieci
    public bool isImmune = false;  // Zmienna okre�laj�ca, czy boss jest odporny

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (!isGlowing && !isImmune) // Dodaj warunek, aby nie obraca� bossa, gdy �wieci lub jest odporny
        {
            if (transform.position.x > player.position.x && isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = false;
            }
            else if (transform.position.x < player.position.x && !isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = true;
            }
        }
    }

    // Metoda, aby ustawi�, �e boss �wieci
    public void SetGlowing(bool glowing)
    {
        isGlowing = glowing;
    }

    // Metoda, aby ustawi�, �e boss jest odporny
    public void SetImmune(bool immune)
    {
        isImmune = immune;
        // Dodaj tutaj kod obs�uguj�cy efekt "Immune", np. zmiana wygl�du bossa
    }
}
