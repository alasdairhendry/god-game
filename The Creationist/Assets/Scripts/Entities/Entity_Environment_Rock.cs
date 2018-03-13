using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Environment_Rock : Entity_Environment {

    public override void Initialize(EntityData data, TerrainEntity.TerrainSegment spawnSegment)
    {
        base.Initialize(data, spawnSegment);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void AddInitialAttributes()
    {
        base.AddInitialAttributes();
        attributes.Add(new Attribute(Attribute.AttributeKey.biomeTundra, 1.0f));
    }

    protected override void AddInitialRadialOptions()
    {
        base.AddInitialRadialOptions();
    }

    protected override void AddInitialAI()
    {
        
    }

    protected override void MonitorAge()
    {
        base.MonitorAge();
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

    public override void PickUp()
    {
        base.PickUp();
    }

    public override void Drop(Vector3 hitPoint)
    {
        base.Drop(hitPoint);
    }
}
