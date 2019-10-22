using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTarget : MonoBehaviour
{
    public Transform target;
    public float speed = 1.0f;
    private Vector3 vec;
    
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }
    
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 0.2f);

        transform.position += transform.forward * speed;
    }
}
