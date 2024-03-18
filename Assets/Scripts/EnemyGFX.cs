using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
    public AIPath aiPath; // Skrypt AIPath, kt�ry jest odpowiedzialny za poruszanie si� wroga

    // Update is called once per frame
    void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f) // Je�li wrogowi porusza si� w prawo
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); // Obr�� wroga w prawo
        }
        else if (aiPath.desiredVelocity.x <= -0.01f) // Je�li wrogowi porusza si� w lewo
        {
            transform.localScale = new Vector3(1f, 1f, 1f); // Obr�� wroga w lewo
        }
    }
}
