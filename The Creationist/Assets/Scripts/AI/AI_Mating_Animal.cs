using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Mating_Animal : AI_Mating {

    //protected override void Start() { base.Start(); }

    //protected override void Update()
    //{
    //    FindMate();
    //}

    //protected override IEnumerator MonitorMating()
    //{
    //    while(true)
    //    {
    //        while (!base.entity.IsOfMatingAge) yield return null;
    //        while (base.entity.IsPickedUp) yield return null;
    //        while (base.isMating) yield return null;
    //        while (!base.allowMating) yield return null;

    //        matingCounter += Time.deltaTime * GameTime.singleton.GameTimeMultipler;

    //        if(matingCounter >= base.entityData.AverageMateDelay)
    //        {
    //            float mateChance = Random.value;

    //            if(mateChance <= base.entityData.MateChance)
    //            {
    //                base.shouldFindMate = true;
    //            }
    //        }

    //        if (shouldFindMate)
    //        {
    //            if(hasTargetMate)
    //            {
    //                if(reachedMateLocation)
    //                {
    //                    if (!isMating)
    //                    {
    //                        StartCoroutine(Mate());
    //                        Debug.Log("MATING");
    //                    }
    //                }
    //            }
    //        }

    //        if(shouldFindMate)
    //        {
    //            if(hasTargetMate)
    //            {
    //                if(!reachedMateLocation)
    //                {
    //                    MoveToTarget();
    //                }
    //            }
    //        }

    //        yield return null;
    //    }


    //}

    //protected override void FindMate()
    //{
    //    if (!base.shouldFindMate) return;
    //    if (base.hasTargetMate) return;
    //    if (base.targetedAsMate) return;
    //    if (base.isMating) return;

    //    List<Entity> entitiesOnTerrainNode = base.entity.TerrainSegment.ReturnEntitiesInNode();
    //    List<Entity> eligibleEntities = new List<Entity>();

    //    foreach (Entity e in entitiesOnTerrainNode)
    //    {
    //        if (e == null) continue;
    //        if (e == base.entity) continue;
    //        if (e.gameObject == null) return;
    //        if (e.AIMating.HasTargetMate) continue;
    //        if (e.AIMating.TargetedAsMate) continue;
    //        if (!e.IsOfMatingAge) continue;            
    //        if (Vector3.Distance(e.transform.position, base.entity.transform.position) > base.entityData.MatingRange) continue;

    //        if (EntityController.singleton.CheckMateMatch(base.entity, e))
    //        {
    //            eligibleEntities.Add(e);
    //        }
    //    }

    //    if (eligibleEntities.Count <= 0) return;    // No eligible mating entities so return 

    //    Entity chosenEntity = eligibleEntities[Random.Range(0, eligibleEntities.Count)];
    //    base.targetMate = chosenEntity;
    //    base.hasTargetMate = true;

    //    if (base.targetMate == base.entity)
    //    {
    //        Debug.LogError("BROBGOSFKO", this);
    //    }

    //    Debug.Log(gameObject.name + " is targeting " + base.targetMate.gameObject.name, this);
    //    targetMate.AIMating.Target(GetComponent<Entity>());

    //}

    //protected override IEnumerator Mate()
    //{
    //    base.OnBeginMate();
    //    targetMate.AIMating.OnBeginMate();

    //    float mateDuration = UnityEngine.Random.Range(1.5f, 3.0f);

    //    base.entity.GetAttributes.Update(Attribute.AttributeKey.status, "Mating");

    //    yield return new WaitForSeconds(mateDuration / GameTime.singleton.GameTimeMultipler);

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
    //        EntityController.singleton.Breed(base.entity, targetMate);
    //    }

    //    targetMate.AIMating.OnCompleteMate();
    //    base.OnCompleteMate();
    //}

    //private void MoveToTarget()
    //{
    //    if (!targetMate)
    //    {
    //        OnCompleteMate();
    //        return;
    //    }
    //    if(targetMate.TerrainSegment.terrain != base.entity.TerrainSegment.terrain)
    //    {
    //        targetMate.AIMating.OnCompleteMate();
    //        OnCompleteMate();
    //        return;
    //    }

    //    if (!allowMating) return;

    //    Vector3 moveDirection = (targetMate.transform.position - GetComponent<Rigidbody>().position).normalized;
    //    GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + moveDirection * base.entityData.MovementSpeed * Time.deltaTime * GameTime.singleton.GameTimeMultipler);

    //    if(Vector3.Distance(targetMate.transform.position, base.entity.transform.position) <= 3.5f)
    //    {
    //        base.reachedMateLocation = true;
    //    }
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
    //    if(targetMate != null)
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
            if (e.GetData.Species != entity.GetData.Species) continue;
            if (!e.GetComponent<AI_Mating>().IsAvailable()) continue;

            if (Vector3.Distance(e.transform.position, base.entity.transform.position) > base.entityData.MatingRange) continue;

            eligibleEntities.Add(e);
        }

        e = eligibleEntities;

        if(eligibleEntities.Count <= 0)
        {
            OnMateComplete();
            return;
        }
        Entity chosenEntity = eligibleEntities[Random.Range(0, eligibleEntities.Count)];
        SetMate(chosenEntity);
        chosenEntity.GetComponent<AI_Mating>().SetAsTargeted();
    }

    protected override void MoveToMate()
    {
        if(shouldFindMate)
        {
            if(hasTargetMate)
            {
                if(targetMate != null)
                {
                    Vector3 moveDirection = (targetMate.transform.position - GetComponent<Rigidbody>().position).normalized;
                    GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + moveDirection * (base.entityData.MovementSpeed * 1.5f) * Time.deltaTime * GameTime.singleton.GameTimeMultipler);

                    if (Vector3.Distance(targetMate.transform.position, base.entity.transform.position) <= 3.5f)
                    {
                        if(!isMating)
                        {
                            StartCoroutine(Mate());
                        }
                    }

                    if (targetMate.IsPickedUp)
                    {
                        if (targetMate != null)
                            targetMate.GetComponent<AI_Mating>().OnMateComplete();
                        OnMateComplete();
                        return;
                    }
                }                
            }
        }
    }

    protected override IEnumerator Mate()
    {
        isMating = true;
        
        if(targetMate == null)
        {
            OnMateComplete();
            yield break;
        }

        yield return StartCoroutine(GameTime.singleton.Yield(Random.Range(1.5f, 3.0f)));

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
                Breeding.singleton.Breed(entity, targetMate, entity.transform.position);
                targetMate.GetComponent<AI_Mating>().OnMateComplete();
                OnMateComplete();
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
