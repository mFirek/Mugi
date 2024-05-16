using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ObjectFallController : MonoBehaviour
{
    public float wait = 0.1f;
    public float destructionDelay = 3f; // OpóŸnienie przed zniszczeniem obiektu
    public float fallSpeed = 10f; // Prêdkoœæ spadania obiektów
    public GameObject fallingObject;
    public List<string> collisionTags = new List<string>(); // Lista tagów obiektów, z którymi ma zderzyæ siê spadaj¹cy obiekt
    public GameObject destructionEffect; // Efekt niszczenia

    void Start()
    {
        InvokeRepeating("Fall", 0, wait);
    }

    void Fall()
    {
        GameObject obj = Instantiate(fallingObject, new Vector3(Random.Range(-10, 10), 10, 0), Quaternion.identity);

        // Dodaj komponent Rigidbody, jeœli nie istnieje
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = obj.AddComponent<Rigidbody2D>();
        }

        // Ustaw prêdkoœæ spadania obiektu
        rb.gravityScale = fallSpeed;

        // Rozpocznij korutynê do niszczenia obiektu po okreœlonym czasie
        StartCoroutine(DestroyObjectDelayed(obj));
    }

    IEnumerator DestroyObjectDelayed(GameObject obj)
    {
        yield return new WaitForSeconds(destructionDelay);

        // SprawdŸ, czy obiekt nie zosta³ ju¿ zniszczony przez zderzenie
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
                // Wywo³aj funkcjê niszczenia obiektu
                DestroyObject(collision.gameObject);
                DestroyObject(gameObject);
                return; // Przerwij pêtlê, jeœli zderzy³ siê z odpowiednim tagiem
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
