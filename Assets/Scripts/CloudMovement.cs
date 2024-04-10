using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed = 2f; // Prêdkoœæ przesuwania chmurki
    public float respawnPositionX = -10f; // Pozycja X, do której chmurka ma wracaæ po dotarciu do koñca ekranu

    private void Update()
    {
        // Przesuwanie chmurki w prawo
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Jeœli chmurka dotknie granicy ekranu
        if (transform.position.x >= Camera.main.aspect * Camera.main.orthographicSize)
        {
            // Przesuñ chmurkê z powrotem na lew¹ stronê ekranu
            transform.position = new Vector3(respawnPositionX, transform.position.y, transform.position.z);
        }
    }
}
