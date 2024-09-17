using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFacePlayer : MonoBehaviour
{
    [SerializeField] private Transform player; // Przypisz tutaj obiekt gracza

    private void Update()
    {
        FacePlayer();
    }

    private void FacePlayer()
    {
        Vector3 direction = player.position - transform.position; // Kierunek od NPC do gracza

        // Jeœli gracz jest po lewej stronie NPC, obróæ NPC w prawo (skala ujemna)
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // NPC patrzy w prawo
        }
        // Jeœli gracz jest po prawej stronie NPC, obróæ NPC w lewo (skala dodatnia)
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // NPC patrzy w lewo
        }
    }
}
