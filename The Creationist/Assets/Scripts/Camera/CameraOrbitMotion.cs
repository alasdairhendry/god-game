using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.ImageEffects;

public class CameraOrbitMotion : MonoBehaviour {

    [SerializeField] private Transform cam;
    [SerializeField] private Vector2 rotationalSpeed = new Vector2(50, 150);
    [SerializeField] private Vector2 rotationalYLimit = new Vector2(-50, 89);
    [SerializeField] private float rotationSmooth = 5.0f;
    
    private float zoom = 1;
    public float ZoomLevel { get { return zoom; } }
    [SerializeField] private float zoomSpeed = 50.0f;
    [SerializeField] private float zoomSmooth = 5.0f;
    [SerializeField] private float zoomLimit = 25.0f;
    public float ZoomLimit { get { return zoomLimit; } }
    float x = 0.0f, y = 0.0f;

    private bool followMode = false;    
    private GameObject followTarget;
    private IInspectable followInspectable;

    private bool shouldOrbit = false;

    void Update ()
    {
        if (!followMode)
        {
            Orbit();           
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                StopFollow();

            if (Input.GetMouseButtonDown(1))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    StopFollow();
                    shouldOrbit = true;
                }
            }

            Follow();
        }

        ZoomInput();
        Zoom();
    }

    private void Orbit()
    {        
        if (Input.GetMouseButtonDown(1))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                shouldOrbit = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            shouldOrbit = false;
        }

        if (shouldOrbit)
        {
            float rotationModifier = Mathf.Lerp(0.1f, 1.0f, Mathf.InverseLerp(zoomLimit, 1.0f, zoom));
            x += Input.GetAxis("Mouse X") * rotationalSpeed.x * Time.deltaTime * rotationModifier;
            y -= Input.GetAxis("Mouse Y") * rotationalSpeed.y * Time.deltaTime * rotationModifier;

            y = ClampAngle(y, rotationalYLimit.x, rotationalYLimit.y);
        }

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSmooth * Time.deltaTime);        
    }

    private void Follow()
    {
        if (followTarget == null) { StopFollow(); return; }

        float rotationModifier = Mathf.Lerp(0.1f, 1.0f, Mathf.InverseLerp(zoomLimit, 1.0f, zoom));        
        Vector3 direction = (transform.position - followTarget.transform.position).normalized;

        Quaternion look = Quaternion.FromToRotation(transform.forward, direction) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, look, rotationSmooth * Time.deltaTime * rotationModifier);
    }

    public void SetFollow(GameObject target)
    {       
        followMode = true;
        followTarget = target;
        if(followTarget.GetComponent<IInspectable>().Target != null)
        {
            followInspectable = followTarget.GetComponent<IInspectable>();
            followInspectable.OnStartInspect();
        }
    }

    public void StopFollow()
    {
        if (followInspectable != null)
        {
            if (followInspectable.Target != null)
            {
                followInspectable.OnStopInspect();
            }
        }
        
        followMode = false;
        followTarget = null;
        followInspectable = null;
    }

    private void ZoomInput()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            zoom = Mathf.Clamp(zoom - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime, zoomLimit, 1.0f);
        }
    }

    private void Zoom()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(zoom, zoom, zoom), zoomSmooth * Time.deltaTime);
        GetComponentInChildren<BlurOptimized>().blurSize = Mathf.Lerp(1.5f, 0.0f, Mathf.InverseLerp(zoomLimit, 1, zoom));
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
