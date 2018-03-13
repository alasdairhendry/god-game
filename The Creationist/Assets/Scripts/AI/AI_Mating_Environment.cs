using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Mating_Environment : AI_Mating {

    //protected override void Start() { base.Start(); }

    //protected override void Update()
    //{
    //    FindMate();
    //}

    //protected override IEnumerator MonitorMating()
    //{
    //    while (true)
    //    {
    //        while (!base.entity.IsOfMatingAge) yield return null;
    //        while (base.entity.IsPickedUp) yield return null;
    //        while (base.isMating) yield return null;
    //        while (!base.allowMating) yield return null;

    //        matingCounter += Time.deltaTime * GameTime.singleton.GameTimeMultipler;

    //        if (matingCounter >= base.entityData.AverageMateDelay)
    //        {
    //            float mateChance = Random.value;

    //            if (mateChance <= base.entityData.MateChance)
    //            {
    //                base.shouldFindMate = true;
    //            }
    //        }

    //        if (shouldFindMate)
    //        {
    //            if (hasTargetMate)
    //            {
    //                StartCoroutine(Mate());
    //            }
    //        }

    //        yield return null;
    //    }


    //}

    //protected override void FindMate()
    //{
    //    if (!base.shouldFindMate) return;
    //    if (base.hasTargetMate) return;

    //    List<Entity> entitiesOnTerrainNode = base.entity.TerrainSegment.ReturnEntitiesInNode();
    //    List<Entity> eligibleEntities = new List<Entity>();

    //    foreach (Entity e in entitiesOnTerrainNode)
    //    {
    //        if (e == null) continue;
    //        if (e == base.entity) continue;
    //        if (e.gameObject == null) continue;
    //        if (e.AIMating.HasTargetMate) continue;
    //        if (e.AIMating.TargetedAsMate) continue;
    //        if (!e.IsOfMatingAge) continue;
    //        if (Vector3.Distance(e.transform.position, base.entity.transform.position) > base.entityData.MatingRange) continue;

    //        if (EntityController.singleton.CheckMateMatch(base.entity, e))
    //        {
    //            eligibleEntities.Add(e);
    //        }

    //        if (eligibleEntities.Count <= 0) return;    // No eligible mating entities so return 

    //        Entity chosenEntity = eligibleEntities[Random.Range(0, eligibleEntities.Count)];
    //        base.targetMate = chosenEntity;



    //        base.hasTargetMate = true;

    //        base.targetMate.AIMating.Target(base.entity);
    //    }
    //}

    //protected override IEnumerator Mate()
    //{
    //    base.OnBeginMate();
    //    targetMate.AIMating.OnBeginMate();

    //    float mateDuration = UnityEngine.Random.Range(1.5f, 3.0f);

    //    base.entity.GetAttributes.Update(Attribute.AttributeKey.status, "Mating");

    //    yield return new WaitForSeconds(mateDuration * GameTime.singleton.DeltaTime);

    //    if (base.targetMate == null)
    //    {
    //        base.OnCompleteMate();
    //        yield break;
    //    }

    //    while (base.entity.IsPickedUp)
    //        yield return null;

    //    while (base.targetMate.IsPickedUp)
    //        yield return null;

    //    while (!base.allowMating)
    //        yield return null;


    //    float fertilityChance = Random.value;

    //    if (fertilityChance <= base.entityData.Fertility)
    //    {
    //        EntityController.singleton.BreedAtPoint(base.entity, targetMate, base.entity.TerrainSegment.FindPointWithinRange(base.entityData.MatingRange / 2, base.entity.transform.position));
    //    }

    //    targetMate.AIMating.OnCompleteMate();
    //    base.OnCompleteMate();
    //}    

    //public override void Target(Entity b)
    //{
    //    base.Target(b);
    //}

    //public override void OnBeginMate()
    //{
    //    base.OnBeginMate();
    //}

    //public override void OnCompleteMate()
    //{
    //    base.OnCompleteMate();
    //}

    //public override void EnableMating() { allowMating = true; }

    //public override void DisableMating() { allowMating = false; }

    //private void OnDisable()
    //{
    //    if (targetMate != null)
    //    {
    //        targetMate.AIMating.OnCompleteMate();
    //    }
    //}

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (isActiveAI) MoveToMate();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override float GetPriority()
    {
        if (shouldFindMate) return 1.0f;
        else return 0.0f;
    }

    public override void Begin()
    {
        base.Begin();
    }

    public override void Stop()
    {
        base.Stop();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnFinish()
    {
        base.OnFinish();
    }

    public override void Reset()
    {
        base.Reset();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void MonitorMatingCounter()
    {
        base.MonitorMatingCounter();        
    }

    [SerializeField] List<Entity> e = new List<Entity>();
    protected override void FindMate()
    {
        List<Entity> entitiesOnTerrainNode = base.entity.TerrainSegment.ReturnEntitiesInNode();
        List<Entity> eligibleEntities = new List<Entity>();

        foreach (Entity e in entitiesOnTerrainNode)
        {
            if (e == null) continue;
            if (e == base.entity) continue;
            if (e.gameObject == null) continue;
            if (!e.GetComponent<AI_Mating>().IsAvailable()) continue;

            if (Vector3.Distance(e.transform.position, base.entity.transform.position) > base.entityData.MatingRange) continue;

            if (EntityController.singleton.CheckMateMatch(base.entity, e))
            {
                eligibleEntities.Add(e);
            }
        }

        e = eligibleEntities;

        if (eligibleEntities.Count <= 0)
        {
            OnMateComplete();
            return;
        }
        Entity chosenEntity = eligibleEntities[Random.Range(0, eligibleEntities.Count)];
        SetMate(chosenEntity);
        chosenEntity.GetComponent<AI_Mating>().SetAsTargeted();
        StartCoroutine(Mate());
    }

    protected override void MoveToMate()
    {
        
    }

    protected override IEnumerator Mate()
    {
        isMating = true;

        yield return StartCoroutine(GameTime.singleton.Yield(Random.Range(1.5f, 3.0f)));

        if (targetMate == null)
        {
            OnMateComplete();
            yield break;
        }

        if (targetMate.IsPickedUp)
        {
            if (targetMate != null)
                targetMate.GetComponent<AI_Mating>().OnMateComplete();

            OnMateComplete();
            yield break;
        }

        if (Random.value >= base.CheckMatingChance())
        {
            float fertilityChance = Random.value;
            if (fertilityChance <= entityData.Fertility)
            {
                Vector3 spawnDestination = Vector3.zero;
                while (spawnDestination == Vector3.zero)
                {
                    float xRange = Random.Range(-entityData.MatingRange / 2.0f, entityData.MatingRange / 2.0f);
                    float zRange = Random.Range(-entityData.MatingRange / 2.0f, entityData.MatingRange / 2.0f);

                    Vector3 randomDirection = Vector3.zero;
                    randomDirection += transform.right * xRange;
                    randomDirection += transform.forward * zRange;

                    Vector3 newDestination = entity.GetHabitatOrigin + randomDirection;

                    Vector3 directionFromPlanet = (Vector3.zero - newDestination).normalized;
                    Ray ray = new Ray(newDestination - (directionFromPlanet * 25.0f), directionFromPlanet);

                    RaycastHit[] hits;

                    hits = Physics.RaycastAll(ray, 35.0f);

                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider.gameObject.tag == "TerrainSegment")
                        {
                            if (entity.TerrainSegment.terrain == hit.collider.gameObject)
                            {
                                spawnDestination = hit.point;
                                EntityController.singleton.BreedAtPoint(entity, targetMate, spawnDestination);
                                targetMate.GetComponent<AI_Mating>().OnMateComplete();
                                OnMateComplete();
                                break;
                            }
                        }
                    }
                    yield return null;
                }
            }
            else
            {
                Debug.Log("Fertility Check Failed ", this);
                targetMate.GetComponent<AI_Mating>().OnMateComplete();
                OnMateComplete();
            }
        }
        else
        {
            Debug.Log("Mating Chance Check Failed ", this);
            targetMate.GetComponent<AI_Mating>().OnMateComplete();
            OnMateComplete();
        }
    }

    public override void OnMateComplete()
    {
        base.OnMateComplete();
    }

    protected override void SetMate(Entity mate)
    {
        base.SetMate(mate);
    }

    public override void SetAsTargeted()
    {
        base.SetAsTargeted();
    }

    public override bool IsAvailable()
    {
        return base.IsAvailable();
    }

}
