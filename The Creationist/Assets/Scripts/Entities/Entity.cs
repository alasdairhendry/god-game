using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IInspectable, IPoolable
{
    protected EntityData _data;
    public EntityData GetData { get { return _data; } }
    protected Animator animator;

    protected AI_Mating aiMating;
    public AI_Mating AIMating { get { return aiMating; } }

    protected TerrainEntity.TerrainSegment terrainSegment;  // Which terrain segment are we currently standing on?
    public TerrainEntity.TerrainSegment TerrainSegment { get { return terrainSegment; } set { terrainSegment = value; } }

    protected float age = 1;  // Days
    protected float lifetime;
    protected float mateCounter = 0.0f; // Counts up to reach our average mate delay in Days
    
    [SerializeField] protected bool isOfMatingAge = false; // Are we current an infant, or can we mate? (Infant is currently set to first 15% of average lifetime
    public bool IsOfMatingAge { get { return isOfMatingAge; } }    

    [SerializeField] protected List<Attribute> attributes = new List<Attribute>();
    protected List<Attribute> Attributes { get { return attributes; } set { attributes = value; } }
    public List<Attribute> GetAttributes { get { return attributes; } }

    [SerializeField] protected List<AI> aiComponents = new List<AI>();    

    protected List<RadialOptionMenu> radialOptions = new List<RadialOptionMenu>();
    public List<RadialOptionMenu> RadialOptions { get { return radialOptions; } }

    protected bool isPickedUp = false;
    public bool IsPickedUp { get { return isPickedUp; } }

    protected Vector3 habitatOrigin = Vector3.zero;
    public Vector3 GetHabitatOrigin { get { return habitatOrigin; } }
    public Vector3 SetHabitatOrigin { set { habitatOrigin = value; } }

    public virtual void Initialize(EntityData data, TerrainEntity.TerrainSegment spawnSegment)
    {
        _data = data;
        terrainSegment = spawnSegment;
        terrainSegment.AddEntityToTerrain(this);
        lifetime = data.AverageLifetime * UnityEngine.Random.Range(0.85f, 1.15f);

        AddInitialAttributes();
        AddInitialRadialOptions();
        AddInitialAI();
        gameObject.name = data.Species + " " + UnityEngine.Random.value;        
    }

    // Use this for initialization
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        MonitorSize();
        MonitorAge();
    }

    protected virtual void AddInitialAttributes()
    {
        Attributes = new List<Attribute>()
        {            
            new Attribute(Attribute.AttributeKey.species, _data.Species, false),
            new Attribute(Attribute.AttributeKey.name, EntityController.singleton.GetRandomAnimalName() + " the " + _data.InfantName, true),
            new Attribute(Attribute.AttributeKey.age, 1.0f, false),
            new Attribute(Attribute.AttributeKey.status, "Idling", false),
            new Attribute(Attribute.AttributeKey.size, 1.0f, false),                       
        };

        foreach (Attribute.AttributeJSONData data in _data.InitialAttributes)
        {
            Attribute.AttributeKey key = (Attribute.AttributeKey)System.Enum.Parse(typeof(Attribute.AttributeKey), data.key);
            Attribute.AttributeType type = (Attribute.AttributeType)System.Enum.Parse(typeof(Attribute.AttributeType), data.type.ToUpper());
            string value = data.value;

            if (type == Attribute.AttributeType.STRING)
            {
                Attribute attr = new Attribute(key, value, data.modifiable);
                Attributes.Add(attr);
            }
            else if (type == Attribute.AttributeType.INT)
            {
                Attribute attr = new Attribute(key, int.Parse(value), data.modifiable);
                Attributes.Add(attr);
            }
            else if (type == Attribute.AttributeType.FLOAT)
            {
                Attribute attr = new Attribute(key, float.Parse(value), data.modifiable);
                Attributes.Add(attr);
            }
        }
    }

    protected virtual void AddInitialRadialOptions()
    {
        radialOptions.Add(new RadialOptionMenu(() =>
        {
            gameObject.AddComponent<EntityPickupControl>();
        }, Resources.Load<Sprite>("Sprites\\RadialOption_PickUp")));

        radialOptions.Add(new RadialOptionMenu(() =>
        {
            GameObject.FindObjectOfType<InspectableController>().OnShowInspectionPanel(this.gameObject);
        }, Resources.Load<Sprite>("Sprites\\RadialOption_Inspect")));
    }

    public virtual void AddRadialOption(RadialOptionMenu option)
    {
        radialOptions.Add(option);
    }

    public virtual void RemoveRadialOption(RadialOptionMenu option)
    {
        radialOptions.Remove(option);
    }

    protected virtual void AddInitialAI()
    {
        foreach (string ai in _data.InitialAI)
        {
            switch (ai)
            {
                case "Movement_DefaultRoaming":
                    gameObject.AddComponent<AI_Movement_DefaultRoaming>();
                    break;

                case "Mating_Animal":
                    gameObject.AddComponent<AI_Mating_Animal>();
                    break;

                case "Mating_Environment":
                    gameObject.AddComponent<AI_Mating_Environment>();
                    break;
            }
        }
    }

    protected virtual void RemoveInitialAI()
    {
        AI[] ai = GetComponents<AI>();
        for (int i = 0; i < ai.Length; i++)
        {
            Destroy(ai[i]);
        }
    }    

    protected virtual void MonitorSize()
    {
        float ageSize = Mathf.Lerp(0.3f, 1.0f, age / (_data.AverageLifetime * 0.45f));
        float attrSize = Mathf.Lerp(0.8f, 1.2f, float.Parse(attributes.FindAttributeByKey(Attribute.AttributeKey.size).GetValue()));
        float result = ageSize * attrSize;
        transform.localScale = new Vector3(result, result, result);
    }

    protected virtual void MonitorAge()
    {
        age += GameTime.singleton.DeltaTime;

        if (age >= _data.AverageLifetime * 0.000010f)
        {
            isOfMatingAge = true;
        }

        if (age >= lifetime)
            EntityPool.singleton.Destroy(_data.EntityDataID, this.gameObject);

        Attributes.Update(Attribute.AttributeKey.age, age);
    }

    GameObject IInspectable.Target { get { if (gameObject != null) return gameObject; else return null; } }
    bool IInspectable.UIFollow { get { return true; } }

    void IInspectable.OnStartInspect()
    {
        if (GetComponentInChildren<MeshRenderer>().GetComponent<Outline>() == null)
        {
            GetComponentInChildren<MeshRenderer>().gameObject.AddComponent<Outline>();
        }

        GetComponentInChildren<Outline>().enabled = true;
    }

    void IInspectable.OnStopInspect()
    {
        Debug.Log("OnStopInspect", this);
        if (GetComponentInChildren<Outline>() != null)
            GetComponentInChildren<Outline>().enabled = false;
    }

    List<Attribute> IInspectable.GetAttributes()
    {
        return Attributes;
    }

    public virtual void PickUp()
    {
        //GetComponent<Rigidbody>().isKinematic = false;        

        GetComponent<GravityAttractee>().StopAttract();
        isPickedUp = true;

        attributes.Update(Attribute.AttributeKey.status, "Floating");

        GetComponent<AIController>().Stop();

        if (terrainSegment != null)
            terrainSegment.RemoveEntityFromTerrain(this);
    }

    public virtual void Drop(Vector3 hitPoint)
    {
        habitatOrigin = hitPoint;

        GetComponent<GravityAttractee>().StartAttract();
        isPickedUp = false;        

        if (terrainSegment != null)
            terrainSegment.AddEntityToTerrain(this);

        GetComponent<AIController>().Play();
    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(habitatOrigin, 1.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(habitatOrigin, _data.RoamingRange * 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(habitatOrigin, _data.RoamingRange);
    }

    void IPoolable.OnInstantiate()
    {
        age = 1;
        lifetime = 0;
        mateCounter = 0.0f;                
        attributes.Clear();
        radialOptions.Clear();
        isPickedUp = false;

        if (_data != null)
            AddInitialAI();
        
        //AI[] ais = GetComponents<AI>();
        //foreach (AI ai in ais)
        //{
        //    ai.Reset();
        //}
    }

    void IPoolable.OnDestroy()
    {
        RemoveInitialAI();
        terrainSegment.RemoveEntityFromTerrain(this);
    }
}