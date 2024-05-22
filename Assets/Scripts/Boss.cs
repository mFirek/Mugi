using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;

    public bool isFlipped = false;

    private bool isGlowing = false; // Dodaj zmienn� okre�laj�c�, czy boss �wieci

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (!isGlowing) // Dodaj warunek, aby nie obraca� bossa, gdy �wieci
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

    // Dodaj metod�, aby ustawi�, �e boss �wieci
    public void SetGlowing(bool glowing)
    {
        isGlowing = glowing;
    }

}
