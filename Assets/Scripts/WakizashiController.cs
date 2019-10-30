using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakizashiController : MonoBehaviour
{
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

    }

    void Update()
    {
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
        
    }
}
