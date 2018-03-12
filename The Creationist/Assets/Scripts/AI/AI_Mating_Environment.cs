using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Mating_Environment : AI_Mating {

    protected override void Start() { base.Start(); }

    protected override void Update()
    {
        FindMate();
    }

    protected override IEnumerator MonitorMating()
    {
        while (true)
        {
            while (!base.entity.IsOfMatingAge) yield return null;
            while (base.entity.IsPickedUp) yield return null;
            while (base.isMating) yield return null;
            while (!base.allowMating) yield return null;

            matingCounter += Time.deltaTime * GameTime.singleton.GameTimeMultipler;

            if (matingCounter >= base.entityData.AverageMateDelay)
            {
                float mateChance = Random.value;

                if (mateChance <= base.entityData.MateChance)
                {
                    base.shouldFindMate = true;
                }
            }

            if (shouldFindMate)
            {
                if (hasTargetMate)
                {
                    StartCoroutine(Mate());
                }
            }

            yield return null;
        }


    }

    protected override void FindMate()
    {
        if (!base.shouldFindMate) return;
        if (base.hasTargetMate) return;

        List<Entity> entitiesOnTerrainNode = base.entity.TerrainSegment.ReturnEntitiesInNode();
        List<Entity> eligibleEntities = new List<Entity>();

        foreach (Entity e in entitiesOnTerrainNode)
        {
            if (e == null) continue;
            if (e == base.entity) continue;
            if (e.gameObject == null) continue;
            if (e.AIMating.HasTargetMate) continue;
            if (e.AIMating.TargetedAsMate) continue;
            if (!e.IsOfMatingAge) continue;
            if (Vector3.Distance(e.transform.position, base.entity.transform.position) > base.entityData.MatingRange) continue;

            if (EntityController.singleton.CheckMateMatch(base.entity, e))
            {
                eligibleEntities.Add(e);
            }

            if (eligibleEntities.Count <= 0) return;    // No eligible mating entities so return 

            Entity chosenEntity = eligibleEntities[Random.Range(0, eligibleEntities.Count)];
            base.targetMate = chosenEntity;



            base.hasTargetMate = true;

            base.targetMate.AIMating.Target(base.entity);
        }
    }

    protected override IEnumerator Mate()
    {
        base.OnBeginMate();
        targetMate.AIMating.OnBeginMate();

        float mateDuration = UnityEngine.Random.Range(1.5f, 3.0f);

        base.entity.GetAttributes.Update(Attribute.AttributeKey.status, "Mating");

        yield return new WaitForSeconds(mateDuration * GameTime.singleton.DeltaTime);

        if (base.targetMate == null)
        {
            base.OnCompleteMate();
            yield break;
        }

        while (base.entity.IsPickedUp)
            yield return null;

        while (base.targetMate.IsPickedUp)
            yield return null;

        while (!base.allowMating)
            yield return null;


        float fertilityChance = Random.value;

        if (fertilityChance <= base.entityData.Fertility)
        {
            EntityController.singleton.BreedAtPoint(base.entity, targetMate, base.entity.TerrainSegment.FindPointWithinRange(base.entityData.MatingRange / 2, base.entity.transform.position));
        }

        targetMate.AIMating.OnCompleteMate();
        base.OnCompleteMate();
    }    

    public override void Target(Entity b)
    {
        base.Target(b);
    }

    public override void OnBeginMate()
    {
        base.OnBeginMate();
    }

    public override void OnCompleteMate()
    {
        base.OnCompleteMate();
    }

    public override void EnableMating() { allowMating = true; }

    public override void DisableMating() { allowMating = false; }

    private void OnDisable()
    {
        if (targetMate != null)
        {
            targetMate.AIMating.OnCompleteMate();
        }
    }
}
