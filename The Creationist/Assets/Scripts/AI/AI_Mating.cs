﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Mating : AI {

    [SerializeField] protected float matingCounter = 0.0f;
    [SerializeField] protected bool shouldFindMate = false;
    [SerializeField] protected bool hasTargetMate = false;
    [SerializeField] protected bool isMating = false;

    [SerializeField] protected bool isTargetedByAnother = false;

    [SerializeField] protected Entity targetMate;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        MonitorMatingCounter();
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

        if (shouldFindMate)
        {
            if (!hasTargetMate)
            {
                if (!isMating)
                {
                    FindMate();
                }
            }
        }
    }

    protected override void OnFinish()
    {
        base.OnFinish();

        hasTargetMate = false;
        isMating = false;
        isTargetedByAnother = false;
        matingCounter = 0.0f;

        if (targetMate != null)
        {
            targetMate.GetComponent<AI_Mating>().OnFinish();
            targetMate = null;
        }
    }

    public override void Reset()
    {
        base.Reset();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected virtual void MonitorMatingCounter()
    {
        if (entity.IsOfMatingAge)
        {
            matingCounter += Time.deltaTime * GameTime.singleton.GameTimeMultipler;

            if (matingCounter >= entityData.AverageMateDelay)
                shouldFindMate = true;
            else shouldFindMate = false;
        }
    }

    protected virtual void FindMate() { }
    
    protected virtual void MoveToMate() { }

    protected virtual IEnumerator Mate() { yield return null; }  
    
    public virtual void OnMateComplete()
    {
        OnFinish();
    }

    protected virtual void SetMate(Entity mate)
    {        
        targetMate = mate;
        hasTargetMate = true;
    }

    protected virtual float CheckMatingChance()
    {


        return 0;
    }

    public virtual void SetAsTargeted()
    {
        isTargetedByAnother = true;
    }

    public virtual bool IsAvailable()
    {
        if (hasTargetMate) return false;
        if (isMating) return false;
        if (isTargetedByAnother) return false;
        if (targetMate) return false;
        if (entity != null)
            if (!entity.IsOfMatingAge) return false;
        return true;
    }
}
