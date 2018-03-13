using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour {

    public static GravityAttractor singleton;
    [SerializeField] public LayerMask planetMask;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

    [SerializeField] private float gravity = -9.8f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Attract(Rigidbody rb, bool attract, bool rotate)
    {        
        Vector3 gravityUp = (rb.gameObject.transform.position - transform.position).normalized;
        Vector3 bodyUp = rb.transform.up;

        if(attract)
        rb.AddForce(gravityUp * gravity * GameTime.singleton.GameTimeMultipler * Time.deltaTime);

        if (!rotate) return;
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * rb.transform.rotation;
        rb.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1000 *  Time.deltaTime * GameTime.singleton.GameTimeMultipler);        
    }
}
