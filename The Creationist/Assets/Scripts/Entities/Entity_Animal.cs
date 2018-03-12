using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Animal : Entity {

    public override void Initialize(EntityData data, TerrainEntity.TerrainSegment spawnSegment)
    {
        base.Initialize(data, spawnSegment);
        gameObject.AddComponent<AI_Movement_Herd>();
        gameObject.AddComponent<AI_Mating_Animal>();
        aiMating = GetComponent<AI_Mating>();
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
        Attributes = new List<Attribute>()
        {
            new Attribute(Attribute.AttributeKey.name, EntityController.singleton.GetRandomAnimalName() + " the " + _data.InfantName),
            new Attribute(Attribute.AttributeKey.age, 1.0f),
            new Attribute(Attribute.AttributeKey.entityType, "Mammal"),
            new Attribute(Attribute.AttributeKey.species, _data.Name),
            new Attribute(Attribute.AttributeKey.status, "Idling"),
            new Attribute(Attribute.AttributeKey.size, 1.0f),
        };
    }

    protected override void AddInitialRadialOptions()
    {
        base.AddInitialRadialOptions();

        radialOptions.Add(new RadialOptionMenu(() =>
        {
            GetComponent<AI_Movement>().ClearDestination();
        }, Resources.Load<Sprite>("Sprites\\RadialOption_ClearDestination")));
    }

    //protected override void MonitorMating()
    //{
    //    base.MonitorMating();
    //}

    //protected override void FindMate()
    //{
    //    base.FindMate();
    //}

    //protected override IEnumerator BeginMate()
    //{
    //    return base.BeginMate();
    //}

    //public override void TargetAsMate()  // Called from other entities who want to mate with us
    //{
    //    base.TargetAsMate();
    //}

    //public override void StopTargetAsMate()    // Called from other entities when they have finished mating with us
    //{
    //    base.StopTargetAsMate();
    //}

    //public override void HasBeenMated()  // Called from other entities when they have mated with us
    //{
    //    base.HasBeenMated();
    //}

    protected override void MonitorAge()
    {
        base.MonitorAge();        
    }
}

//public override void Initialize(EntityData data, TerrainEntity.TerrainSegment spawnSegment)
//{
//    base.Initialize(data, spawnSegment);
//}

//protected override void Start()
//{
//    base.Start();
//}

//protected override void Update()
//{
//    base.Update();
//}

//protected override void FixedUpdate()
//{
//    base.FixedUpdate();
//}

//protected override void AddInitialAttributes()
//{
//    Attributes = new List<Attribute>()
//        {
//            new Attribute(Attribute.AttributeKey.name, EntityController.singleton.GetRandomAnimalName() + " the " + _data.InfantName),
//            new Attribute(Attribute.AttributeKey.age, 1.0f),
//            new Attribute(Attribute.AttributeKey.entityType, "Mammal"),
//            new Attribute(Attribute.AttributeKey.species, _data.Name),
//            new Attribute(Attribute.AttributeKey.status, "Idling"),
//            new Attribute(Attribute.AttributeKey.size, 1.0f),
//        };
//}

//protected override IEnumerator MonitorRoaming()
//{
//    return base.MonitorRoaming();
//}

//protected override void MonitorMating()
//{
//    base.MonitorMating();
//}

//protected override void FindMate()
//{
//    base.FindMate();
//}

//protected override IEnumerator BeginMate()
//{
//    return base.BeginMate();
//}

//public override void TargetAsMate()  // Called from other entities who want to mate with us
//{
//    base.TargetAsMate();
//}

//public override void StopTargetAsMate()    // Called from other entities when they have finished mating with us
//{
//    base.StopTargetAsMate();
//}

//public override void HasBeenMated()  // Called from other entities when they have mated with us
//{
//    base.HasBeenMated();
//}

//protected override void MonitorAge()
//{
//    base.MonitorAge();
//}

//protected override void SetDestination(Vector3 location)
//{
//    base.SetDestination(location);
//}

//protected override void MoveToDestination()
//{
//    base.MoveToDestination();
//}
