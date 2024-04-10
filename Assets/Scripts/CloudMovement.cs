using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed = 2f; // Pr�dko�� przesuwania chmurki
    public float respawnPositionX = -10f; // Pozycja X, do kt�rej chmurka ma wraca� po dotarciu do ko�ca ekranu

    private void Update()
    {
        // Przesuwanie chmurki w prawo
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Je�li chmurka dotknie granicy ekranu
        if (transform.position.x >= Camera.main.aspect * Camera.main.orthographicSize)
        {
            // Przesu� chmurk� z powrotem na lew� stron� ekranu
            transform.position = new Vector3(respawnPositionX, transform.position.y, transform.position.z);
        }
    }
}
