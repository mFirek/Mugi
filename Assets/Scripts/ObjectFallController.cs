using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFallController : MonoBehaviour
{
    public float wait = 0.1f;
    public GameObject fallingObject;

    void Start()
    {
        InvokeRepeating("fall", 0, wait);
    }

    
    void fall()
        {
            Instantiate(fallingObject, new Vector3(Random.Range (-10,10),10,0), Quaternion.identity);
        }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
