using UnityEngine;

public class BridgeTrigger : MonoBehaviour
{
    public Animator bridgeAnimator;
    public string triggeringTag = "Kula";  // Tag pocisku

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggeringTag))
        {
            // Uruchom animacjê obni¿ania mostu
            bridgeAnimator.SetTrigger("LowerBridge");
        }
    }
}
