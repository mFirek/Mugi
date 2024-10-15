using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 10f; // Prêdkoœæ kamery
    public float yOffset = 0f; // Bazowy offset kamery
    public float minYOffset = -2f; // Minimalny offset (gdy gracz jest blisko sufitu)
    public float maxYOffset = 2f;  // Maksymalny offset (gdy gracz jest na pod³odze)
    public float ceilingThreshold = 5f; // Pozycja Y powy¿ej której kamera obni¿a siê (gracz jest blisko sufitu)
    public float floorThreshold = -5f;  // Pozycja Y poni¿ej której kamera podnosi siê (gracz jest blisko pod³ogi)
    public Transform target;

    // Granice boczne kamery
    public float minX; // Minimalna wartoœæ X kamery
    public float maxX; // Maksymalna wartoœæ X kamery

    // Update is called once per frame
    void Update()
    {
        // SprawdŸ, gdzie gracz siê znajduje
        float playerY = target.position.y;

        // Dostosuj offset kamery w zale¿noœci od wysokoœci gracza
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

        // Ustaw now¹ pozycjê kamery z ograniczeniem bocznych œcian
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);

        // Ograniczenie ruchu kamery w osi X do wartoœci minX i maxX
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

        // Przypisz now¹ pozycjê kamery
        transform.position = Vector3.Lerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}
