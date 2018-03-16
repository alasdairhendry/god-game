using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectableController : MonoBehaviour {

    GameObject inspectable;

	// Update is called once per frame
	void Update () {
        MonitorInput();
	}

    private void MonitorInput()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;

        //    if(Physics.Raycast(ray, out hit))
        //    {
        //        IInspectable targetIInspecatble = hit.collider.gameObject.GetComponent<IInspectable>();

        //        if (targetIInspecatble != null)
        //        {
        //            if(targetIInspecatble == inspectable)
        //            {
        //                OnHideInspectionPanel();
        //                inspectable = null;
        //                return;
        //            }

        //            try
        //            {
        //                if (inspectable != null)
        //                {
        //                    if (inspectable.Target != null)
        //                    {
        //                        inspectable.OnStopInspect();
        //                    }
        //                }
        //            }
        //            catch { }

        //            inspectable = hit.collider.gameObject.GetComponent<IInspectable>();
        //            inspectable.OnStartInspect();
        //            OnShowInspectionPanel();
        //        }
        //        else
        //        {
        //            if (inspectable == null) return;
        //            if (inspectable != null)
        //            OnHideInspectionPanel();
        //        }
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inspectable == null) return;
            OnStopInspect();
        }
    }

    public void OnStartInspect(GameObject inspectable)
    {
        if (this.inspectable != null)
        {
            if (this.inspectable.GetComponent<IInspectable>() != null)
            {                
                this.inspectable.GetComponent<IInspectable>().OnStopInspect();
            }
        }

        this.inspectable = inspectable;
        this.inspectable.GetComponent<IInspectable>().OnStartInspect();
        //FindObjectOfType<InspectionCanvas>().ShowInspectionPanel(inspectable.GetComponent<IInspectable>().UIFollow, inspectable);
        FindObjectOfType<InspectionCanvas>().Open(inspectable);
    }

    private void OnStopInspect()
    {
        FindObjectOfType<InspectionCanvas>().Close();
        if (inspectable != null)
        {
            if (inspectable.GetComponent<IInspectable>().Target != null)
            {
                inspectable.GetComponent<IInspectable>().OnStopInspect();
            }
        }
    }
}
