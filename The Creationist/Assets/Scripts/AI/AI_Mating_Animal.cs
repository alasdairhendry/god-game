using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AI_Mating_Animal : AI_Mating {

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isActiveAI) MoveToMate();
    }

    public override float GetPriority()
    {
        if (shouldFindMate)
        {
            if (isTargetedByAnother)
                return 0.0f;

            Logger.LogSelected("Get Priority 01", gameObject,this );
            return 1.0f;
        }

        else
        {
            Logger.LogSelected("Get Priority 00", gameObject,this);
            return 0.0f;
        }
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
            if (e.gameObject.activeSelf == false) continue;
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
                if (targetMate != null)
                {
                    if (targetMate.gameObject.activeSelf)
                    {                        
                        //GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + (moveDirection * (base.entityData.MovementSpeed)) * (Time.deltaTime * GameTime.singleton.GameTimeMultipler));
                        //GetComponent<Rigidbody>().MovePosition(transform.position + moveDirection);

                        //Vector3 moveDirection = (targetMate.transform.position - transform.position).normalized;
                        //transform.position = Vector3.Lerp(transform.position, transform.position + moveDirection, Time.deltaTime);                       

                        Logger.LogSelected("Moving To Mate", this.gameObject, this);

                        if (Vector3.Distance(targetMate.transform.position, base.entity.transform.position) <= 1.0f)
                        {
                            if (!isMating)
                            {
                                StartCoroutine(Mate());
                                Debug.Log("Close enough to start mating");
                            }
                        }
                        else
                        {
                            Vector3 moveDirection = (targetMate.transform.position - GetComponent<Rigidbody>().position).normalized;
                            GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + moveDirection * entityData.MovementSpeed * Time.deltaTime * GameTime.singleton.GameTimeMultipler);
                        }

                        if (targetMate.IsPickedUp)
                        {
                            if (targetMate != null)
                                targetMate.GetComponent<AI_Mating>().OnMateComplete();
                            OnMateComplete();
                            return;
                        }
                    }
                    else
                    {
                        // Mate has been deleted
                        OnMateComplete();
                    }
                }
                else
                {
                    // Mate has been deleted
                    OnMateComplete();
                }
            }
        }
    }

    protected override IEnumerator Mate()
    {
        isMating = true;

        if (targetMate == null)
        {
            OnMateComplete();
            yield break;
        }

        if (targetMate.gameObject.activeSelf == false)
        {
            OnMateComplete();
            yield break;
        }

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
                Breeding.singleton.Breed(entity, targetMate, entity.transform.position);
                if (targetMate != null)
                    targetMate.GetComponent<AI_Mating>().OnMateComplete();
                OnMateComplete();
            }
            else
            {                
                if (targetMate != null)
                    targetMate.GetComponent<AI_Mating>().OnMateComplete();
                OnMateComplete();
            }
        }
        else
        {            
            if (targetMate != null)
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
