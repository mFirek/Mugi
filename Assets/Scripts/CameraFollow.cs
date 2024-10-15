using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 10f; // Pr�dko�� kamery
    public float yOffset = 0f; // Bazowy offset kamery
    public float minYOffset = -2f; // Minimalny offset (gdy gracz jest blisko sufitu)
    public float maxYOffset = 2f;  // Maksymalny offset (gdy gracz jest na pod�odze)
    public float ceilingThreshold = 5f; // Pozycja Y powy�ej kt�rej kamera obni�a si� (gracz jest blisko sufitu)
    public float floorThreshold = -5f;  // Pozycja Y poni�ej kt�rej kamera podnosi si� (gracz jest blisko pod�ogi)
    public Transform target;

    // Granice boczne kamery
    public float minX; // Minimalna warto�� X kamery
    public float maxX; // Maksymalna warto�� X kamery

    // Update is called once per frame
    void Update()
    {
        // Sprawd�, gdzie gracz si� znajduje
        float playerY = target.position.y;

        // Dostosuj offset kamery w zale�no�ci od wysoko�ci gracza
        if (playerY > ceilingThreshold)
        {
            yOffset = Mathf.Lerp(yOffset, minYOffset, FollowSpeed * Time.deltaTime);
        }
        else if (playerY < floorThreshold)
        {
            yOffset = Mathf.Lerp(yOffset, maxYOffset, FollowSpeed * Time.deltaTime);
        }
        else
        {
            yOffset = Mathf.Lerp(yOffset, 0f, FollowSpeed * Time.deltaTime);
        }

        // Ustaw now� pozycj� kamery z ograniczeniem bocznych �cian
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);

        // Ograniczenie ruchu kamery w osi X do warto�ci minX i maxX
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

        // Przypisz now� pozycj� kamery
        transform.position = Vector3.Lerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}
