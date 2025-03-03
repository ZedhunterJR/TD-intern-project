using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    public static Transform MainCamTransform { get { return Instance.gameObject.transform; } }
    public static Camera MainCam 
    { 
        get { return Instance.gameObject.transform.Find("cam_object").GetComponent<Camera>(); } 
    }
    private void Awake()
    {
        Instance = this;
        camTransform = transform.Find("cam_object");
        cam = camTransform.GetComponent<Camera>();
    }
    private Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private Camera cam;
    private Vector3 difference;
    private Vector3 origin;
    private bool drag = false;

    private Vector2 MaxPos()
    {
        return new(100, 100);
    }
    private Vector2 MinPos()
    {
        return new(0, 0);
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = Vector3.zero + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.unscaledDeltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = Vector3.zero;
        }
    }
    void LateUpdate()
    { 
        if (Input.GetMouseButton(1))
        {
            difference = (cam.ScreenToWorldPoint(Input.mousePosition)) - cam.transform.position;
            if (!drag)
            {
                drag = true;
                origin = cam.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }
        if (drag)
        {
            transform.position = origin - difference;
        }

        Vector3 constrain = new Vector3(Mathf.Clamp(cam.transform.position.x, MinPos().x, MaxPos().x),
            Mathf.Clamp(cam.transform.position.y, MinPos().y, MaxPos().y), -10);
        cam.transform.position = constrain;

    }
    public void Shake(float value)
    {
        shakeDuration = value;
    }

}
