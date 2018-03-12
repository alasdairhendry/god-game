using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementNew : MonoBehaviour {

    [SerializeField] private float rotationalSpeed = 100.0f;
    [SerializeField] private float panSpeed = 100.0f;
    [SerializeField] private float zoomSpeed = 100.0f;
    [SerializeField] private float zoomClamp = 5.0f;

    [SerializeField] private Transform target;
    private Transform rotationTarget;

    private float distanceToPlanet = 0.0f;

	// Use this for initialization
	void Start () {
        //target = GravityAttractor.singleton.transform.parent;        
	}
	
	// Update is called once per frame
	void Update () {
        Pan();
        Rotate();
        Zoom();
        distanceToPlanet = Vector3.Distance(target.position, transform.position);
	}

    private void Pan()
    {
        if(Input.GetMouseButton(1))
        {
            //transform.position += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0.0f) * -panSpeed * Time.deltaTime;
        }
    }

    private void Rotate()
    {
        //fullTarget.transform.eulerAngles += new Vector3(0.0f, Input.GetAxis("Horizontal"), 0.0f) * rotationalSpeed * Time.deltaTime;   
        if (Input.GetMouseButton(1))
        {
            target.Rotate(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0.0f) * rotationalSpeed * Time.deltaTime * (distanceToPlanet / 260.0f), Space.World);
        }
    }

    private void Zoom()
    {
        Vector3 newPosition = transform.position + (transform.forward * zoomSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime);

        Ray ray = new Ray(newPosition, transform.forward);
        RaycastHit raycastHit;
        bool hit = false;

        if (Physics.Raycast(ray, out raycastHit, zoomClamp))
        {
            hit = true;
        }

        if (!hit)
        {
            transform.position += transform.forward * zoomSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
        }
    }
}
