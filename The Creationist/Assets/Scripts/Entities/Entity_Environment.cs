using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Environment : Entity {

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
    }

    protected override void AddInitialRadialOptions()
    {
        base.AddInitialRadialOptions();
    }

    protected override void AddInitialAI()
    {
        base.AddInitialAI();
    }

    protected override void RemoveInitialAI()
    {
        base.RemoveInitialAI();
    }

    protected override void MonitorAge()
    {
        base.MonitorAge();
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "TerrainSegment")
        //{
        //    GetComponent<Rigidbody>().isKinematic = true;
        //    GetComponent<GravityAttractee>().enabled = false;
        //}
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
