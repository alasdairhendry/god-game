using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Mating_Environment : AI_Mating {

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
            if (e.gameObject.activeSelf == false) continue;
            if (e.GetData.Species != entity.GetData.Species) continue;
            if (!e.GetComponent<AI_Mating>().IsAvailable()) continue;

            if (Vector3.Distance(e.transform.position, base.entity.transform.position) > base.entityData.MatingRange) continue;

            eligibleEntities.Add(e);            
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

        if (targetMate.gameObject.activeSelf == false)
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
                                Breeding.singleton.Breed(entity, targetMate, spawnDestination);
                                if (targetMate != null)
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
                if (targetMate != null)
                    targetMate.GetComponent<AI_Mating>().OnMateComplete();
                OnMateComplete();
            }
        }
        else
        {
            Debug.Log("Mating Chance Check Failed ", this);
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
