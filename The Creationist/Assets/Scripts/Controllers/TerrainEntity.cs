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

    [SerializeField] public Color plainsColour;
    [SerializeField] public Color ForestColour;
    [SerializeField] public Color GrasslandsColour;
    [SerializeField] public Color TundraColour;    

    // Use this for initialization
    void Start()
    {        
        GenerateTerrain();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            foreach (TerrainSegment t in terrainSegments)
            {
                StartCoroutine(t.ColouriseNodes());
            }

        if (Input.GetKeyDown(KeyCode.G))
            foreach (TerrainSegment t in terrainSegments)
            {
                StartCoroutine(t.ColouriseTerrain());
            }
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

        private Mesh mesh;
        List<Color> vertexColours = new List<Color>();

        private bool isColourisingNodes = false;
        private bool isColourisingTerrain = false;

        public TerrainSegment(List<TerrainNode> nodes, GameObject terrain)
        {
            this.nodes = nodes;
            this.terrain = terrain;
            this.mesh = terrain.GetComponent<MeshFilter>().mesh;            

            for (int i = 0; i < nodes.Count; i++)
            {
                vertexColours.Add(TerrainEntity.singleton.plainsColour);
            }

            mesh.SetColors(vertexColours);
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
                if (e.GetData.Species == entityName)
                    eligibleEntities.Add(e);
            }

            return eligibleEntities;
        }

        public IEnumerator ColouriseNodes()
        {
            if (isColourisingNodes)
                yield break;

            isColourisingNodes = true;

            while (true)
            {
                int i = 0;

                while (i < nodes.Count)
                {
                    nodes[i].CheckColour();                    
                    i++;
                    yield return null;
                }
            }
        }

        public IEnumerator ColouriseTerrain()
        {
            if (isColourisingTerrain)
                yield break;

            isColourisingTerrain = true;

            while (true)
            {
                int i = 0;

                while (i < nodes.Count)
                {
                    vertexColours[i] = nodes[i].GetColor();                    
                    i++;
                    yield return null;
                }

                mesh.SetColors(vertexColours);

            }            

            //isColourisingTerrain = false;
        }

        //------------------------------------------------------------------------\\

        //private List<Triangle> triangles = new List<Triangle>();
        //private Mesh mesh;
        //private Color[] colours;

        //private void InitializeTerrainTriangulisation()
        //{
        //    CalculateTriangles();

        //    colours = new Color[mesh.vertexCount];
        //    List<Color> coloursList = new List<Color>();
        //    for (int i = 0; i < colours.Length; i++)
        //    {
        //        colours[i] = Color.green;
        //        coloursList.Add(colours[i]);
        //    }
        //    mesh.SetColors(coloursList);
        //}

        //private void CalculateTriangles()
        //{
        //    int[] t = mesh.GetTriangles(0);
        //    List<int> e = new List<int>();
        //    for (int i = 0; i < t.Length; i++)
        //    {
        //        e.Add(t[i]);
        //    }

        //    for (int i = 0; i < t.Length; i += 3)
        //    {
        //        Triangle tri = new Triangle(mesh, terrain.transform, t[i], t[i + 1], t[i + 2]);
        //        triangles.Add(tri);
        //    }
        //}

        //public IEnumerator Repaint()
        //{
        //    while (true)
        //    {
        //        yield return new WaitForSeconds(1.5f);
        //        int triCounter = 0;

        //        while (triCounter < triangles.Count)
        //        {
        //            triangles[triCounter].GetColour(ref colours);

        //            triCounter++;
        //        }

        //        List<Color> coloursList = new List<Color>();

        //        for (int i = 0; i < colours.Length; i++)
        //        {                    
        //            coloursList.Add(colours[i]);
        //        }

        //        mesh.SetColors(coloursList);

        //        yield return null;
        //    }
        //}

        //public IEnumerator TriCalculate()
        //{
        //    while (true)
        //    {
        //        int x = 0;

        //        while (x < triangles.Count)
        //        {
        //            Debug.Log("Calculating Triangle " + x);
        //            triangles[x].CalculateColour();
        //            x++;
        //            yield return null;
        //        }

        //        yield return new WaitForSeconds(1.0f);

        //        yield return null;
        //    }
        //}

        //public class Triangle
        //{
        //    public int vertexIndex01;   // Also the uv index
        //    public int vertexIndex02;
        //    public int vertexIndex03;

        //    public Vector3 vertex01;
        //    public Vector3 vertex02;
        //    public Vector3 vertex03;
            
        //    public bool draw = false;

        //    Mesh mesh;
        //    private Color terrainColour;

        //    public Triangle(Mesh mesh, Transform transform, int v01, int v02, int v03)
        //    {
        //        this.mesh = mesh;
        //        this.vertexIndex01 = v01;
        //        this.vertexIndex02 = v02;
        //        this.vertexIndex03 = v03;
        //        terrainColour = TerrainEntity.singleton.plainsColour;

        //        this.vertex01 = transform.TransformPoint(mesh.vertices[v01]);
        //        this.vertex02 = transform.TransformPoint(mesh.vertices[v02]);
        //        this.vertex03 = transform.TransformPoint(mesh.vertices[v03]);                
        //    }

        //    public void SetUVRed(ref Color[] newColours)
        //    {
        //        newColours[vertexIndex01] = Color.red;
        //        newColours[vertexIndex02] = Color.red;
        //        newColours[vertexIndex03] = Color.red;
        //    }

        //    public void GetColour(ref Color[] colours)
        //    {
        //        colours[vertexIndex01] = terrainColour;
        //        colours[vertexIndex02] = terrainColour;
        //        colours[vertexIndex03] = terrainColour;
        //    }

        //    public void CalculateColour()
        //    {
        //        TerrainInspector.TerrainBiomeRatio ratio = TerrainInspector.singleton.InspectTerrainAtPoint(vertex01, 5.0f);
        //        terrainColour = TerrainEntity.singleton.plainsColour;

        //        if (ratio.GetHighestRatio() == TerrainInspector.TerrainBiomeRatio.HighestRatio.None)
        //        {
        //            terrainColour = TerrainEntity.singleton.plainsColour;
        //        }
        //        else if (ratio.GetHighestRatio() == TerrainInspector.TerrainBiomeRatio.HighestRatio.Forest)
        //        {
        //            terrainColour = Color.Lerp(TerrainEntity.singleton.plainsColour, TerrainEntity.singleton.ForestColour, ratio.GetHighestRatioValue() / 1.0f);
        //        }
        //        else if (ratio.GetHighestRatio() == TerrainInspector.TerrainBiomeRatio.HighestRatio.Grasslands)
        //        {
        //            terrainColour = Color.Lerp(TerrainEntity.singleton.plainsColour, TerrainEntity.singleton.GrasslandsColour, ratio.GetHighestRatioValue() / 1.0f);
        //        }
        //        else if (ratio.GetHighestRatio() == TerrainInspector.TerrainBiomeRatio.HighestRatio.Tundra)
        //        {
        //            terrainColour = Color.Lerp(TerrainEntity.singleton.plainsColour, TerrainEntity.singleton.TundraColour, ratio.GetHighestRatioValue() / 1.0f);
        //        }
        //    }
        //}

    }

    [System.Serializable]
    public class TerrainNode
    {
        public Vector3 worldPosition;
        public GameObject parent;

        private Color nodeColour = TerrainEntity.singleton.plainsColour;

        public TerrainNode(Vector3 worldPosition, GameObject parent)
        {
            this.worldPosition = worldPosition;
            this.parent = parent;
        }

        public void CheckColour()
        {
            TerrainInspector.TerrainBiomeRatio ratio = TerrainInspector.singleton.InspectTerrainAtPoint(worldPosition, 10.0f);
            nodeColour = TerrainEntity.singleton.plainsColour;

            if (ratio.GetHighestRatio() == TerrainInspector.TerrainBiomeRatio.HighestRatio.None)
            {
                nodeColour = TerrainEntity.singleton.plainsColour;
            }
            else if (ratio.GetHighestRatio() == TerrainInspector.TerrainBiomeRatio.HighestRatio.Forest)
            {
                nodeColour = Color.Lerp(TerrainEntity.singleton.plainsColour, TerrainEntity.singleton.ForestColour, ratio.GetHighestRatioValue() / 1.0f);
            }
            else if (ratio.GetHighestRatio() == TerrainInspector.TerrainBiomeRatio.HighestRatio.Grasslands)
            {
                nodeColour = Color.Lerp(TerrainEntity.singleton.plainsColour, TerrainEntity.singleton.GrasslandsColour, ratio.GetHighestRatioValue() / 1.0f);
            }
            else if (ratio.GetHighestRatio() == TerrainInspector.TerrainBiomeRatio.HighestRatio.Tundra)
            {
                nodeColour = Color.Lerp(TerrainEntity.singleton.plainsColour, TerrainEntity.singleton.TundraColour, ratio.GetHighestRatioValue() / 1.0f);
            }
        }

        public Color GetColor()
        {
            return nodeColour;
        }
    }
}
