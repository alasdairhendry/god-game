using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EntityPickupControl : MonoBehaviour {

    private Vector3 requestedPosition = Vector3.zero;    

    private void Start()
    {
        GetComponent<Entity>().PickUp();
        GetComponent<Collider>().isTrigger = true;        
    }

    private void Update () {
        GetPosition();
        MovePosition();

        if(Input.GetMouseButtonDown(0))
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;

            //if (Physics.Raycast(ray, out hit))
            //{
            //    if (hit.collider.gameObject.tag == "TerrainSegment")
            //    {
            //        GetComponent<Entity>().TerrainSegment = TerrainEntity.singleton.FindSegment(hit.collider.gameObject);
            //        Drop(hit.point);
            //        Debug.Log(hit.point);
            //        Destroy(this);
            //    }
            //}

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;


            hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.tag == "TerrainSegment")
                {
                    GetComponent<Entity>().TerrainSegment = TerrainEntity.singleton.FindSegment(hit.collider.gameObject);
                    Drop(hit.point);                    
                    Destroy(this);
                    break;
                }
            }
        }
	}

    private void GetPosition()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;


        hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag == "TerrainSegment")
            {
                Vector3 direction = (Vector3.zero - hit.point).normalized;
                requestedPosition = hit.point - (direction * Mathf.Lerp(1.5f, 20.0f, Mathf.InverseLerp(FindObjectOfType<CameraOrbitMotion>().ZoomLimit, 1.0f, FindObjectOfType<CameraOrbitMotion>().ZoomLevel)));
                break;
            }
        }
    }

    private void MovePosition()
    {
        transform.position = Vector3.Lerp(transform.position, requestedPosition, 25 * Time.deltaTime);
    }

    private void Drop(Vector3 hitPoint)
    {
        GetComponent<Entity>().Drop(hitPoint);
        GetComponent<Collider>().isTrigger = false;
    }

    private void OnDestroy()
    {
        //GetComponent<Entity>().Drop();
        //GetComponent<Collider>().isTrigger = false;
    }
}
