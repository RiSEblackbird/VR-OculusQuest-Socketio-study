﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerController : MonoBehaviour
{
    public float Angle = 1f;
    public float DashSpeed = 5f;
    public float SlowSpeed = 1.5f;
    
    // リアルタイム通信向けのパラメーター
    Transform oldHead = null;
    Transform currentHead = null;
    Vector3 oldPosition;
    Vector3 currentPosition;
    Quaternion oldRotation;
    Quaternion currentRotaion;

    void Reset()
    {
        oldHead = GetComponentInChildren<OVRCameraRig>().transform.Find("TrackingSpace/CenterEyeAnchor");
        oldPosition = transform.position;
        oldRotation = transform.rotation;
    }

    void Start()
    {
        currentHead = oldHead;
        currentPosition = oldPosition;
        currentRotaion = oldRotation;
    }
    
    void Update()
    {
        // Forward move
        if (Input.GetKey(KeyCode.W) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp))
        {
            var forward = oldHead.forward;
            forward.y = 0;
            transform.position += forward.normalized * Time.deltaTime;
        }
        // Back move
        if (Input.GetKey(KeyCode.S) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
        {
            var forward = oldHead.forward;
            forward.y = 0;
            transform.position -= forward.normalized * Time.deltaTime;
        }

        // 追記：Left move
        if (Input.GetKey(KeyCode.A) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft))
        {
            var right = oldHead.right;
            right.y = 0;
            transform.position -= right.normalized * Time.deltaTime;
        }
        // 追記：Right move
        if (Input.GetKey(KeyCode.D) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
        {
            var right = oldHead.right;
            right.y = 0;
            transform.position += right.normalized * Time.deltaTime;
        }

        // Left rotate
        if (Input.GetKey(KeyCode.Q) || OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft))
        {
            transform.Rotate(new Vector3(0, -Angle, 0));
        }
        // Right rotate
        if (Input.GetKey(KeyCode.E) || OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight))
        {
            transform.Rotate(new Vector3(0, Angle, 0));
        }
        
    }
}