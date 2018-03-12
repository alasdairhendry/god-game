using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEntity : MonoBehaviour {

    public static TerrainEntity singleton;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

    [SerializeField] private Transform targetParent;
    [SerializeField] private List<TerrainSegment> terrainSegments = new List<TerrainSegment>();
    [SerializeField] private bool drawNodes = false;
    [SerializeField] private Color drawColour = Color.red;

    // Use this for initialization
    void Start()
    {
        GenerateTerrain();    
    }

    private void GenerateTerrain()
    {
        foreach (Transform g in targetParent)
        {
            MeshFilter m = g.GetComponent<MeshFilter>();
            TerrainSegment entity = new TerrainSegment(CalculateVertices(m), m.gameObject);
            terrainSegments.Add(entity);
        }
    }

    private List<TerrainNode> CalculateVertices(MeshFilter mesh)
    {
        Vector3[] verts = mesh.mesh.vertices;
        List<TerrainNode> nodes = new List<TerrainNode>();

        for (int i = 0; i < verts.Length; i++)
        {
            TerrainNode node = new TerrainNode(mesh.gameObject.transform.TransformPoint(verts[i]), mesh.gameObject);
            nodes.Add(node);
        }

        return nodes;
    }

    public TerrainSegment FindSegment(GameObject g)
    {        
        foreach (TerrainSegment s in terrainSegments)
        {
            if (s.terrain == g)
            {
                return s;
            }
        }

        return null;
    }

    public Vector3 ReturnRandomSegmentLocation(TerrainSegment segment)
    {
        int index = UnityEngine.Random.Range(0, segment.nodes.Count);
        return segment.nodes[index].worldPosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawNodes) return;

        Gizmos.color = drawColour;

        foreach (TerrainSegment e in terrainSegments)
        {
            foreach (TerrainNode v in e.nodes)
            {
                Gizmos.DrawSphere(v.worldPosition, 0.2f);
            }
        }
    }

    [System.Serializable]
    public class TerrainSegment
    {
        public List<TerrainNode> nodes = new List<TerrainNode>();
        public GameObject terrain;
        [SerializeField] private List<Entity> entitiesInNode = new List<Entity>();

        public TerrainSegment(List<TerrainNode> nodes, GameObject terrain)
        {
            this.nodes = nodes;
            this.terrain = terrain;
        }

        public Vector3 FindPointWithinRange(float range, Vector3 from)
        {
            Mesh mesh = terrain.GetComponent<MeshFilter>().mesh;
            Transform meshT = terrain.transform;
            List<Vector3> eligibleLocations = new List<Vector3>();

            for (int i = 0; i < mesh.vertexCount; i++)
            {
                if (Vector3.Distance(from, meshT.TransformPoint(mesh.vertices[i])) < range)
                {
                    eligibleLocations.Add(meshT.TransformPoint(mesh.vertices[i]));                    
                }
            }

            if (eligibleLocations.Count > 0)
                return eligibleLocations[UnityEngine.Random.Range(0, eligibleLocations.Count)];
            else
                return from;
        }

        public void AddEntityToTerrain(Entity entity)
        {
            entitiesInNode.Add(entity);
        }

        public void RemoveEntityFromTerrain(Entity entity)
        {
            entitiesInNode.Remove(entity);
        }

        public List<Entity> ReturnEntitiesInNode() { return entitiesInNode; }

        public List<Entity> ReturnEntitiesByType(string entityName)
        {
            List<Entity> eligibleEntities = new List<Entity>();

            foreach (Entity e in entitiesInNode)
            {
                if (e.GetData.Name == entityName)
                    eligibleEntities.Add(e);
            }

            return eligibleEntities;
        }
    }

    [System.Serializable]
    public class TerrainNode
    {
        public Vector3 worldPosition;
        public GameObject parent;

        public TerrainNode(Vector3 worldPosition, GameObject parent)
        {
            this.worldPosition = worldPosition;
            this.parent = parent;
        }
    }
}
