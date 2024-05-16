using UnityEngine;

public class DeactivateObjectOnTrigger : MonoBehaviour
{
    public GameObject objectToDeactivate; // obiekt, kt�ry chcesz dezaktywowa�


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // sprawdzenie, czy obiekt jest graczem
        {
            objectToDeactivate.SetActive(false); // dezaktywacja innego obiektu
        }
    }

}
