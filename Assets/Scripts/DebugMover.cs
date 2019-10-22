using UnityEngine;

// 本アプリ用に調整

public class DebugMover : MonoBehaviour
{
    // private CharacterController Controller;
    // private Vector3 MoveThrottle = Vector3.zero;

    [SerializeField]
    Transform Head = null;
    public const float Angle = 2;
    public const float DashSpeed = 60f;
    public const float SlowSpeed = 30f;
    // public const float JumpPower = 50f;

    void Start()
    {
        // Controller = GetComponent<CharacterController>();
        // MoveThrottle = Vector3.zero;
    }

    void Reset()
    {
        Head = GetComponentInChildren<OVRCameraRig>().transform.Find("TrackingSpace/CenterEyeAnchor");
    }
    
    float Scale
    {
        get
        {
            return IsPressTrigger ? DashSpeed : IsPressGrip ? SlowSpeed : 30f;
        }
    }

    bool IsPressTrigger
    {
        get
        {
            return Input.GetKey(KeyCode.LeftShift);
            // || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        }
    }
    bool IsPressGrip
    {
        get
        {
            return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftAlt);
            //  || OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) || OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);
        }
    }

    void Update()
    {
        // Forward move
        if (Input.GetKey(KeyCode.W) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp))
        {
            var forward = Head.forward;
            forward.y = 0;
            transform.position += forward.normalized * Time.deltaTime * Scale;
        }
        // Back move
        if (Input.GetKey(KeyCode.S) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
        {
            var forward = Head.forward;
            forward.y = 0;
            transform.position -= forward.normalized * Time.deltaTime * Scale;
        }

        // 追記：Left move
        if (Input.GetKey(KeyCode.A) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft))
        {
            var right = Head.right;
            right.y = 0;
            transform.position -= right.normalized * Time.deltaTime * Scale;
        }
        // 追記：Right move
        if (Input.GetKey(KeyCode.D) || OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
        {
            var right = Head.right;
            right.y = 0;
            transform.position += right.normalized * Time.deltaTime * Scale;
        }

        /* 追記：Jump simple 保留
        if (Controller.isGrounded)
        {
            MoveThrottle = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.B) || OVRInput.Get(OVRInput.Button.SecondaryThumbstick) || OVRInput.Get(OVRInput.Button.Two))
        { 
            MoveThrottle += new Vector3(0, JumpPower, 0);
        }
        else
        {
            MoveThrottle += new Vector3(0, -10f, 0);
        }
        */
        

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

        /*
        // Up move
        if (Input.GetKeyDown(KeyCode.K) || OVRInput.GetDown(OVRInput.Button.Four) || OVRInput.GetDown(OVRInput.Button.Two))
        {
            transform.position += Vector3.up * Scale;
        }
        // Down move
        if (Input.GetKeyDown(KeyCode.J) || OVRInput.GetDown(OVRInput.Button.Three) || OVRInput.GetDown(OVRInput.Button.One))
        {
            transform.position -= Vector3.up * Scale;
        }
        */
    }
}
