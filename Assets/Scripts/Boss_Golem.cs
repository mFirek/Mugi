using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Golem : MonoBehaviour
{
    public Transform player;
    public bool isFlipped = false;

    private bool isGlowing = false; // Zmienna okreœlaj¹ca, czy boss œwieci
    public bool isImmune = false;  // Zmienna okreœlaj¹ca, czy boss jest odporny

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (!isGlowing && !isImmune) // Dodaj warunek, aby nie obracaæ bossa, gdy œwieci lub jest odporny
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

    // Metoda, aby ustawiæ, ¿e boss œwieci
    public void SetGlowing(bool glowing)
    {
        isGlowing = glowing;
    }

    // Metoda, aby ustawiæ, ¿e boss jest odporny
    public void SetImmune(bool immune)
    {
        isImmune = immune;
        // Dodaj tutaj kod obs³uguj¹cy efekt "Immune", np. zmiana wygl¹du bossa
    }
}
