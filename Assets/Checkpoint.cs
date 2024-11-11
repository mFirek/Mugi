using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private PlayerCheckpointAnalytics playerCheckpointAnalytics;


   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           
            GameManager.Instance.UpdateSpawnPoint(transform.position);

        }
    }
}
