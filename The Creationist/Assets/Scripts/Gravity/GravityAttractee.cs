using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractee : MonoBehaviour {

    private bool allowAttract = true;
    private bool allowRotate = true;

    // Update is called once per frame
    void FixedUpdate () {
        GravityAttractor.singleton.Attract(this.GetComponent<Rigidbody>(), allowAttract, allowRotate);
	}

    public void StartAttract()
    {
        allowAttract = true;
    }

    public void StopAttract()
    {
        allowAttract = false;
    }

    public void StartRotate()
    {
        allowRotate = true;
    }

    public void StopRotate()
    {
        allowRotate = false;
    }
}
