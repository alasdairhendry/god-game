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
    
    protected bool isOfMatingAge = false; // Are we current an infant, or can we mate? (Infant is currently set to first 15% of average lifetime
    public bool IsOfMatingAge { get { return isOfMatingAge; } }    

    protected Vector3 targetDestination = new Vector3(0, 0, 0);
    [SerializeField] protected List<Attribute> attributes = new List<Attribute>();
    protected List<Attribute> Attributes { get { return attributes; } set { attributes = value; } }
    public List<Attribute> GetAttributes { get { return attributes; } }

    protected List<RadialOptionMenu> radialOptions = new List<RadialOptionMenu>();
    public List<RadialOptionMenu> RadialOptions { get { return radialOptions; } }

    protected bool isPickedUp = false;
    public bool IsPickedUp { get { return isPickedUp; } }

    public virtual void Initialize(EntityData data, TerrainEntity.TerrainSegment spawnSegment)
    {
        _data = data;
        terrainSegment = spawnSegment;
        terrainSegment.AddEntityToTerrain(this);
        lifetime = data.AverageLifetime * UnityEngine.Random.Range(0.85f, 1.15f);

        AddInitialAttributes();
        AddInitialRadialOptions();
        gameObject.name = data.Name + " " + UnityEngine.Random.value;
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
            new Attribute(Attribute.AttributeKey.entityType, "Fake Entity"),
            new Attribute(Attribute.AttributeKey.name, "New Name"),
            new Attribute(Attribute.AttributeKey.age, 1.0f),
            new Attribute(Attribute.AttributeKey.species, "Fake Species"),
            new Attribute(Attribute.AttributeKey.status, "Idling")
        };
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

    protected virtual void AddInitialAI()
    {

    }

    protected virtual void RemoveInitialAI()
    {

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

        if (age >= _data.AverageLifetime * 0.10f)
        {
            isOfMatingAge = true;
        }

        if (age >= lifetime)
            EntityPool.singleton.Destroy(_data.EntityDataID, this.gameObject);

        Attributes.Update(Attribute.AttributeKey.age, age);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetDestination, 2f);
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
        GetComponent<GravityAttractee>().StopAttract();
        isPickedUp = true;

        attributes.Update(Attribute.AttributeKey.status, "Floating");

        if (aiMating != null)
            aiMating.DisableMating();

        //if (targetMate != null)
        //{
        //    //targetMate.StopTargetAsMate();
        //    //targetMate.HasBeenMated();
        //    targetMate.IsMating = false;
        //    targetMate = null;
        //}

        //SetDestination(Vector3.zero);

        if (terrainSegment != null)
            terrainSegment.RemoveEntityFromTerrain(this);
    }

    public virtual void Drop()
    {
        GetComponent<GravityAttractee>().StartAttract();
        isPickedUp = false;

        attributes.Update(Attribute.AttributeKey.status, "Idling");

        if (terrainSegment != null)
            terrainSegment.AddEntityToTerrain(this);

        if (aiMating != null)
            aiMating.EnableMating();
    }

    void IPoolable.OnInstantiate()
    {
        age = 1;
        lifetime = 0;
        mateCounter = 0.0f;        
        targetDestination = Vector3.zero;
        attributes.Clear();
        radialOptions.Clear();
        isPickedUp = false;

        AI[] ais = GetComponents<AI>();
        foreach (AI ai in ais)
        {
            ai.Reset();
        }
    }
}