using UnityEngine;

public class ShieldCollision : MonoBehaviour
{
    public delegate void ShieldHitHandler();
    public event ShieldHitHandler OnShieldHit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called with: " + other.tag); // Debugowanie

        if (other.CompareTag("Kula"))
        {
            Debug.Log("Shield hit by Kula"); // Debugowanie
            OnShieldHit?.Invoke();
        }
    }
}
