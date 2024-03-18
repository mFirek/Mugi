using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
    public AIPath aiPath; // Skrypt AIPath, który jest odpowiedzialny za poruszanie siê wroga

    // Update is called once per frame
    void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f) // Jeœli wrogowi porusza siê w prawo
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); // Obróæ wroga w prawo
        }
        else if (aiPath.desiredVelocity.x <= -0.01f) // Jeœli wrogowi porusza siê w lewo
        {
            transform.localScale = new Vector3(1f, 1f, 1f); // Obróæ wroga w lewo
        }
    }
}
