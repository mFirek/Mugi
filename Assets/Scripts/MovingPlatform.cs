using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;
    private int i;

    void Start()
    {
        if (points.Length > 0)
        {
            transform.position = points[startingPoint].position;
        }
        else
        {
            Debug.LogError("Brak przypisanych punkt�w do zmiennej points!");
        }
    }

    void Update()
    {
        
        if (Vector2.Distance(transform.position, points[i].position) < 0.2f)      
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y > transform.position.y-0.3f)
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}