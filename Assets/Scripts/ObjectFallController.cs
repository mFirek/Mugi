using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ObjectFallController : MonoBehaviour
{
    public float wait = 0.1f;
    public float destructionDelay = 3f; // Op�nienie przed zniszczeniem obiektu
    public float fallSpeed = 10f; // Pr�dko�� spadania obiekt�w
    public GameObject fallingObject;
    public List<string> collisionTags = new List<string>(); // Lista tag�w obiekt�w, z kt�rymi ma zderzy� si� spadaj�cy obiekt
    public GameObject destructionEffect; // Efekt niszczenia

    void Start()
    {
        InvokeRepeating("Fall", 0, wait);
    }

    void Fall()
    {
        GameObject obj = Instantiate(fallingObject, new Vector3(Random.Range(-10, 10), 10, 0), Quaternion.identity);

        // Dodaj komponent Rigidbody, je�li nie istnieje
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = obj.AddComponent<Rigidbody2D>();
        }

        // Ustaw pr�dko�� spadania obiektu
        rb.gravityScale = fallSpeed;

        // Rozpocznij korutyn� do niszczenia obiektu po okre�lonym czasie
        StartCoroutine(DestroyObjectDelayed(obj));
    }

    IEnumerator DestroyObjectDelayed(GameObject obj)
    {
        yield return new WaitForSeconds(destructionDelay);

        // Sprawd�, czy obiekt nie zosta� ju� zniszczony przez zderzenie
        if (obj != null)
        {
            DestroyObject(obj);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (string tag in collisionTags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                // Wywo�aj funkcj� niszczenia obiektu
                DestroyObject(collision.gameObject);
                DestroyObject(gameObject);
                return; // Przerwij p�tl�, je�li zderzy� si� z odpowiednim tagiem
            }
        }
    }

    void DestroyObject(GameObject obj)
    {
        // Tworzenie efektu niszczenia
        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, obj.transform.position, Quaternion.identity);
        }

        // Niszczenie obiektu
        Destroy(obj);
    }
}
