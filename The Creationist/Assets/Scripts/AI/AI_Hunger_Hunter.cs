using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Hunger_Hunter : AI_Hunger {

    private Entity targetEntity;
    private int retryCount = 0;

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
    }

    public override float GetPriority()
    {
        return base.GetPriority();
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
        FindTarget();
    }

    private void FindTarget()
    {
        string targetType = entity.GetAttributes.FindValueByName("Diet");

        Entity[] entities = GameObject.FindObjectsOfType<Entity>();
        List<Entity> eligibleEntities = new List<Entity>();

        foreach (Entity e in entities)
        {
            if (e == null) continue;

            if (e.gameObject.activeSelf == false) continue;

            if (Vector3.Distance(e.transform.position, transform.position) > entityData.RoamingRange) continue;

            if (e.TerrainSegment.terrain != entity.TerrainSegment.terrain) continue;

            if (e.IsPickedUp) continue;            

            if (e.GetData.Species.ToLower() != targetType.ToLower()) continue;

            eligibleEntities.Add(e);
        }

        if (eligibleEntities.Count <= 0)
        {
            Retry();
            return;
        }

        // We have eligible targets
        targetEntity = eligibleEntities[Random.Range(0, eligibleEntities.Count)];

        // We have found a target
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        float timeSinceLastAttack = 0.0f;
        float targetHealth = 1.0f;        

        while(true)
        {
            if (!isActiveAI) { Debug.Log("Not Active"); OnFinish(); yield break; }

            if (targetEntity == null)
            {
                Retry();
                yield break;
            }

            if (targetEntity.gameObject.activeSelf == false)
            {
                Retry();
                yield break;
            }

            if (targetEntity.IsPickedUp)
            {
                Retry();
                yield break;
            }

            entity.GetAttributes.Update(Attribute.AttributeKey.status, ("Hunting ") + targetEntity.GetAttributes.FindAttributeByKey(Attribute.AttributeKey.name).GetValue());

            timeSinceLastAttack += GameTime.singleton.DeltaTime;

            Vector3 destination = targetEntity.transform.position;

            // Move towards the target if they are further than 1 meter
            if(Vector3.Distance(transform.position, destination) >= 1.0f)
            {
                Vector3 moveDirection = (destination - GetComponent<Rigidbody>().position).normalized;
                GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + moveDirection * (entityData.MovementSpeed * 2.5f) * Time.deltaTime * GameTime.singleton.GameTimeMultipler);
            }
            else    // We are close to the target
            {
                if(timeSinceLastAttack >= 1.5f)
                {
                    Debug.Log("ATTACK");
                    targetHealth -= 0.4f;
                    timeSinceLastAttack = 0.0f;

                    if(targetHealth <= 0)
                    {
                        Debug.Log("TARGET DEAD");
                        entity.GetAttributes.Update(Attribute.AttributeKey.hunger, 1.0f);
                        EntityPool.singleton.Destroy(targetEntity.GetData.EntityDataID, targetEntity.gameObject);
                        OnFinish();
                        yield break;
                    }
                }
            }

            yield return null;
        }
    }

    private void Retry()
    {        
        if(retryCount >= 3)
        {
            // TODO - Add negative buff Unhappy because no food?????
            OnFinish();
            return;
        }

        Debug.Log("RETRYING");

        retryCount++;
        OnStart();
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
}
