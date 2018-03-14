using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainInspector : MonoBehaviour {

    public static TerrainInspector singleton;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(2))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;

            hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.tag == "TerrainSegment")
                {
                    current = InspectTerrainAtPoint(hit.point, 5.0f);
                    currentPoint = hit.point;
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(currentPoint, 5.0f);
    }

    [SerializeField] private TerrainBiomeRatio current;
    private Vector3 currentPoint;

    public TerrainBiomeRatio InspectTerrainAtPoint(Vector3 position, float range)
    {
        float forestCount = 0.0f;
        float grasslandsCount = 0.0f;
        float tundraCount = 0.0f;

        float forestValue = 0.0f;
        float grasslandsValue = 0.0f;
        float tundraValue = 0.0f;

        RaycastHit[] hits;

        hits = Physics.SphereCastAll(position, range, (Vector3.zero - position).normalized);

        foreach (RaycastHit hit in hits)
        {
            if(hit.collider.gameObject.GetComponent<Entity>() != null)
            {
                Entity e = hit.collider.gameObject.GetComponent<Entity>();

                if (e.GetAttributes.FindAttributeByKey(Attribute.AttributeKey.biomeForest) != null)
                {
                    float v = e.GetAttributes.FindAttributeByKey(Attribute.AttributeKey.biomeForest).FloatValue;
                    forestValue += v;
                    forestCount++;
                }
                if (e.GetAttributes.FindAttributeByKey(Attribute.AttributeKey.biomeGrasslands) != null)
                {
                    float v = e.GetAttributes.FindAttributeByKey(Attribute.AttributeKey.biomeGrasslands).FloatValue;
                    grasslandsValue += v;
                    grasslandsCount++;
                }
                if (e.GetAttributes.FindAttributeByKey(Attribute.AttributeKey.biomeTundra) != null)
                {
                    float v = e.GetAttributes.FindAttributeByKey(Attribute.AttributeKey.biomeTundra).FloatValue;
                    tundraValue += v;
                    tundraCount++;
                }
            }
        }

        float overallCount = forestCount + grasslandsCount + tundraCount;
        if(overallCount <= 0)
        {
            TerrainBiomeRatio ratio = new TerrainBiomeRatio(0.0f, 0.0f, 0.0f);
            return ratio;            
        }
        else
        {
            TerrainBiomeRatio ratio = new TerrainBiomeRatio(forestValue / overallCount, grasslandsValue / overallCount, tundraValue / overallCount);
            return ratio;
        }
    }

    [System.Serializable]
    public class TerrainBiomeRatio
    {
        [SerializeField] private Attribute forestValue;
        [SerializeField] private Attribute grasslandsValue;
        [SerializeField] private Attribute tundraValue;

        public enum HighestRatio { None, Forest, Grasslands, Tundra };
        private HighestRatio highestRatio = HighestRatio.None;
        private float highestRatioValue = 0.0f;

        public TerrainBiomeRatio(float forest, float grasslands, float tundra)
        {
            forestValue = new Attribute(Attribute.AttributeKey.biomeForest, forest, false);
            grasslandsValue = new Attribute(Attribute.AttributeKey.biomeGrasslands, grasslands, false);
            tundraValue = new Attribute(Attribute.AttributeKey.biomeTundra, tundra, false);

            if(forest == 0 && grasslands == 0 && tundra == 0)
            {
                highestRatio = HighestRatio.None;
                highestRatioValue = 0.0f;
            }
            else if (forest >= grasslands && forest >= tundra)
            {
                highestRatio = HighestRatio.Forest;
                highestRatioValue = forest;
            }
            else if (grasslands >= forest && grasslands >= tundra)
            {
                highestRatio = HighestRatio.Grasslands;
                highestRatioValue = grasslands;
            }
            else if (tundra >= forest && tundra >= grasslands)
            {
                highestRatio = HighestRatio.Tundra;
                highestRatioValue = tundra;
            }
        }

        public HighestRatio GetHighestRatio()
        {
            return highestRatio;
        }

        public float GetHighestRatioValue()
        {
            return highestRatioValue;
        }
    }

}
