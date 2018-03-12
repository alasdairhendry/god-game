using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    private Rigidbody rigidbody;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }

    void LateUpdate()
    {
        if (target)
        {
            if (Input.GetMouseButton(1))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                Quaternion rotation = Quaternion.Euler(y, x, 0);

                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 250, distanceMin, distanceMax);

                RaycastHit hit;
                if (Physics.Linecast(target.position, transform.position, out hit))
                {
                    distance -= hit.distance;
                }
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance + target.position;

                transform.rotation = rotation;
                transform.position = position;
            }
        }
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

 //   [SerializeField] private float zoomClamp = 1.0f;
 //   [SerializeField] private float zoomSpeed = 15.0f;

 //   [SerializeField] private float rotationSpeed = 30.0f;

 //   private Vector3 directionToPlanet = new Vector3(0, 0, 0);
 //   private GameObject planet;

	//// Use this for initialization
	//void Start () {
 //       planet = GravityAttractor.singleton.gameObject;
	//}
	
	//// Update is called once per frame
	//void Update () {
 //       CalculateDirection();
 //       Zoom();
 //       Rotate();
	//}

 //   private void CalculateDirection()
 //   {
 //       directionToPlanet = GravityAttractor.singleton.gameObject.transform.position - transform.position;
 //   }

 //   private void Zoom()
 //   {
 //       Vector3 newPosition = transform.position + (transform.forward * zoomSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime);

 //       Ray ray = new Ray(newPosition, transform.forward);
 //       RaycastHit raycastHit;
 //       bool hit = false;

 //       if (Physics.Raycast(ray, out raycastHit, zoomClamp))
 //       {
 //           hit = true;
 //       }

 //       if(!hit)
 //       {
 //           transform.position += transform.forward * zoomSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
 //       }
 //   }

 //   private void Rotate()
 //   {
 //       //if (Input.GetMouseButton(1))
 //       //{
 //       //    float h = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
 //       //    float v = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

 //       //    Transform target = planet.transform.parent.transform;

 //       //    if (target.eulerAngles.z + v <= 0.1f || target.eulerAngles.z + v >= 179.9f)
 //       //    {
 //       //        v = 0;
 //       //    }

 //       //    target.eulerAngles += new Vector3(v, h, 0);
 //       //}
 //       //planet.transform.parent.transform.localEulerAngles += new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0.0f) * rotationSpeed * Time.deltaTime;

 //       planet.transform.parent.transform.eulerAngles += new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0.0f) * rotationSpeed * Time.deltaTime;
 //   }
