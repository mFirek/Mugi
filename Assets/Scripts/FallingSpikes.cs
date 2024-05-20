using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikes : MonoBehaviour
{
    public GameObject spikes; // Przypisz obiekt kolców w Inspectorze
    public float fallSpeed = 5f; // Prêdkoœæ opadania kolców

    private bool isTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTriggered = true;
        }
    }

    void Update()
    {
        if (isTriggered)
        {
            spikes.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Kolce zderzy³y siê z: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Teren"))
        {
            Destroy(spikes);
        }
    }

}