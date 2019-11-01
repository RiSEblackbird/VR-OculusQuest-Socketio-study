using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCubeController : MonoBehaviour
{
    public Transform target;
    public float speed = 1.0f;
    private Vector3 vec;
    public bool isLocalEnemy = false;
    
    Vector3 oldPosition;
    Vector3 currentPosition;
    Quaternion oldRotation;
    Quaternion currentRotation;
    
    void Start()
    {

        oldPosition = transform.position;
        oldRotation = transform.rotation;

        currentPosition = transform.position;
        currentRotation = transform.rotation;

        // target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (!isLocalEnemy)
        {
            return;
        }

        currentPosition = transform.position;
        currentRotation = transform.rotation;

        if (currentPosition != oldPosition)
        {
            oldPosition = currentPosition;
        }

        if (currentRotation != oldRotation)
        {
            oldRotation = currentRotation;
        }

        // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 0.2f);

        transform.position += transform.forward * speed;
    }
}
