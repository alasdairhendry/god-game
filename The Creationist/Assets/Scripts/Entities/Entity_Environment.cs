using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Environment : Entity {

    public override void Initialize(EntityData data, TerrainEntity.TerrainSegment spawnSegment)
    {
        base.Initialize(data, spawnSegment);
        gameObject.AddComponent<AI_Mating_Environment>();
        base.aiMating = GetComponent<AI_Mating_Environment>();
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
            new Attribute(Attribute.AttributeKey.entityType, "Environment"),
            new Attribute(Attribute.AttributeKey.species, _data.Name),
            new Attribute(Attribute.AttributeKey.size, 1.0f)
        };
    }

    //protected override void MonitorMating()
    //{
    //    // Check we are of mating age
    //    if (!isOfMatingAge) return;

    //    // Monitor when we should mate or not 
    //    mateCounter += GameTime.singleton.DeltaTime;

    //    if (mateCounter >= _data.AverageMateDelay)
    //    {
    //        float mateChance = UnityEngine.Random.value;

    //        if (mateChance < _data.MateChance)
    //        {
    //            shouldFindMate = true;
    //        }
    //    }

    //    // Initiate Mating if criteria are met
    //    if (shouldFindMate)
    //    {
    //        if (hasTargetMate)
    //        {
    //            if (!isMating)
    //            {
    //                StartCoroutine(BeginMate());
    //                isMating = true;
    //            }           
    //        }
    //    }
    //}

    //protected override void FindMate()
    //{
    //    if (!shouldFindMate) return;
    //    if (targetedAsMate) return;
    //    if (hasTargetMate) return;
    //    if (!isOfMatingAge) return;

    //    Entity[] entities = GameObject.FindObjectsOfType<Entity>();
    //    List<Entity> eligibleEntities = new List<Entity>();

    //    foreach (Entity e in entities)
    //    {
    //        if (e == this) continue;
    //        if (e.TargetedAsMate) continue;
    //        if (e.HasTargetMate) continue;
    //        if (!e.IsOfMatingAge) continue;

    //        if (EntityController.singleton.CheckMateMatch(this, e))
    //        {
    //            float distance = Vector3.Distance(transform.position, e.transform.position);

    //            if (distance < _data.MatingRange)
    //            {
    //                eligibleEntities.Add(e);
    //            }
    //        }
    //    }

    //    if (eligibleEntities.Count <= 0)
    //        return;

    //    Entity chosenEntity = eligibleEntities[(UnityEngine.Random.Range(0, eligibleEntities.Count))];
    //    targetMate = chosenEntity;
    //    chosenEntity.TargetAsMate();
    //    //SetDestination(targetMate.transform.position);
    //    hasTargetMate = true;
    //}

    //protected override IEnumerator BeginMate()
    //{
    //    float mateDuration = UnityEngine.Random.Range(1.5f, 3.0f);
    //    animator.SetBool("isMating", true);

    //    //Debug.Log("Yield should have been " + mateDuration + " but it is " + mateDuration / GameTime.singleton.GameTimeMultipler);
    //    yield return new WaitForSeconds(mateDuration * GameTime.singleton.DeltaTime);

    //    while (isPickedUp)
    //        yield return null;

    //    EntityController.singleton.BreedAtPoint(this, targetMate, terrainSegment.FindPointWithinRange(_data.MatingRange / 2, transform.position));
    //    shouldFindMate = false;
    //    hasTargetMate = false;
    //    isMating = false;
    //    reachedMateLocation = false;
    //    targetMate.StopTargetAsMate();
    //    targetMate.HasBeenMated();        
    //    targetMate = null;
    //    mateCounter = 0.0f;
    //    animator.SetBool("isMating", false);
    //}

    //public override void TargetAsMate()  // Called from other entities who want to mate with us
    //{
    //    targetedAsMate = true;
    //}

    //public override void StopTargetAsMate()    // Called from other entities when they have finished mating with us
    //{
    //    targetedAsMate = false;
    //}

    //public override void HasBeenMated()  // Called from other entities when they have mated with us
    //{
    //    shouldFindMate = false;
    //    hasTargetMate = false;
    //    isMating = false;
    //    reachedMateLocation = false;
    //    targetMate = null;
    //    mateCounter = 0.0f;
    //    animator.SetBool("isMating", false);
    //}

    protected override void MonitorAge()
    {
        age += GameTime.singleton.DeltaTime;

        if (age >= _data.AverageLifetime * 0.10f)
        {
            isOfMatingAge = true;
        }

        if (age >= lifetime)
            Destroy(gameObject);

        Attributes.Update(Attribute.AttributeKey.age, age);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "TerrainSegment")
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<GravityAttractee>().enabled = false;
        }
    }

    public override void PickUp()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<GravityAttractee>().enabled = true;

        GetComponent<GravityAttractee>().StopAttract();
        isPickedUp = true;

        if (aiMating != null)
            aiMating.DisableMating();
    }

    public override void Drop()
    {
        GetComponent<GravityAttractee>().StartAttract();
        isPickedUp = false;

        if (aiMating != null)
            aiMating.EnableMating();
    }
}
