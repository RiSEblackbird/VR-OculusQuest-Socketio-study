using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerController : MonoBehaviour
{
    public float Angle = 1f;
    public float DashSpeed = 5f;
    public float SlowSpeed = 1.5f;
    public bool isLocalPlayer = false;
    
    // リアルタイム通信向けのパラメーター
    Transform oldHead = null;
    Transform currentHead = null;
    Vector3 oldHeadPosition;
    Vector3 currentHeadPosition;
    Vector3 oldPosition;
    Vector3 currentPosition;
    Quaternion oldHeadRotation;
    Quaternion currentHeadRotation;
    Quaternion oldRotation;
    Quaternion currentRotation;

    
    void Reset()
    {
        oldHead = GetComponentInChildren<OVRCameraRig>().transform.Find("TrackingSpace/CenterEyeAnchor");
        oldHeadPosition = oldHead.position;
        oldHeadRotation = transform.rotation;
        oldPosition = transform.position;
        oldRotation = transform.rotation;
    }
    /*
    void Start()
    {
        currentHead = oldHead;
        currentHeadPosition = oldHeadPosition;
        currentHeadRotation = oldHeadRotation;
        currentPosition = oldPosition;
        currentRotation = oldRotation;
    }
    */
    void Update()
    {
        // キャラクターが自分であること
        if (!isLocalPlayer)
        {
            return;
        }
       
        currentHead = GetComponentInChildren<OVRCameraRig>().transform.Find("TrackingSpace/CenterEyeAnchor");
        currentHeadPosition = oldHeadPosition;
        currentHeadRotation = oldHeadRotation;
        currentPosition = transform.position;
        currentRotation = transform.rotation;

        if (currentHeadPosition != oldHeadPosition)
        {
            NetworkManager.instance.GetComponent<NetworkManager>().CommandHeadMove(oldHead.position);
            oldHeadPosition = currentHeadPosition;
        }

        if (currentRotation != oldRotation)
        {
            NetworkManager.instance.GetComponent<NetworkManager>().CommandHeadTurn(transform.rotation);
            oldHeadRotation = currentHeadRotation;
        }

        if (currentPosition != oldPosition)
        {
            NetworkManager.instance.GetComponent<NetworkManager>().CommandMove(transform.position);
            oldPosition = currentPosition;
        }

        if (currentRotation != oldRotation)
        {
            NetworkManager.instance.GetComponent<NetworkManager>().CommandTurn(transform.rotation);
            oldRotation = currentRotation;
        }

        // Forward move
        if (Input.GetKey(KeyCode.W) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp))
        {
            var forward = currentHead.forward;
            forward.y = 0;
            transform.position += forward.normalized * Time.deltaTime;
        }
        
        // Back move
        if (Input.GetKey(KeyCode.S) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
        {
            var forward = currentHead.forward;
            forward.y = 0;
            transform.position -= forward.normalized * Time.deltaTime;
        }

        // 追記：Left move
        if (Input.GetKey(KeyCode.A) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft))
        {
            var right = currentHead.right;
            right.y = 0;
            transform.position -= right.normalized * Time.deltaTime;
        }
        // 追記：Right move
        if (Input.GetKey(KeyCode.D) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
        {
            var right = currentHead.right;
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