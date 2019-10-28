using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wakizashi : MonoBehaviour
{
    [HideInInspector]
    public GameObject playerFrom;
    
    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(playerFrom, 10);
        }

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
