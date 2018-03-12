using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_EntityEnvironment_DrawBiome : MonoBehaviour {

    [SerializeField] float initialRange;
    [SerializeField] float maxRange;
    [SerializeField] float rangeIncreaseSpeed;
    [SerializeField] private GameObject target;

    // Use this for initialization
    void Start () {
        target = GameObject.Find("SphericalWorldVisualTerrain");
	}
	
	// Update is called once per frame
	void Update () {
        if (initialRange < maxRange)
        {
            initialRange += rangeIncreaseSpeed * Time.deltaTime * GameTime.singleton.GameTimeMultipler;
        }

        if (initialRange >= maxRange)
            initialRange = maxRange;

        //Debug.Log(FindPointWithinRange(initialRange, transform.position));
        StartCoroutine(Find());
	}

    private IEnumerator Find()
    {
        while(true)
        {
            yield return FindPointWithinRange(initialRange, transform.position);
        }
    }

    public IEnumerator FindPointWithinRange(float range, Vector3 from)
    {
        Mesh mesh = target.GetComponent<MeshFilter>().mesh;
        Transform meshT = target.transform;
        List<Vector3> eligibleLocations = new List<Vector3>();

        int i = 0;

        while(i < mesh.vertexCount)
        {
            if (Vector3.Distance(from, meshT.TransformPoint(mesh.vertices[i])) < range)
            {
                eligibleLocations.Add(meshT.TransformPoint(mesh.vertices[i]));
            }

            Debug.Log("WOrking");

            i++;
            yield return null;
        }

        Debug.Log(eligibleLocations[UnityEngine.Random.Range(0, eligibleLocations.Count)]);

        //if (eligibleLocations.Count > 0)
        //    return eligibleLocations[UnityEngine.Random.Range(0, eligibleLocations.Count)];
        //else
        //    return from;
    }
}
