using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Mating : AI {

    //protected bool allowMating = true;

    //protected float matingCounter = 0.0f;

    //protected bool shouldFindMate = false;  // Should we look for an eligible mate?
    //public bool ShouldFindMate { get { return shouldFindMate; } set { shouldFindMate = value; } }

    //protected bool hasTargetMate = false;   // We have targeted an entity
    //public bool HasTargetMate { get { return hasTargetMate; } }

    //protected bool reachedMateLocation = false; // Are we next to our mate?
    //public bool ReachedMateLocation { get { return reachedMateLocation; } }

    //protected bool isMating = false;    // Are we currently mating? 
    //public bool IsMating { get { return isMating; } set { isMating = value; } }

    //protected Entity targetMate;    // We have targeted this entity to mate with

    //protected bool targetedAsMate = false;  // Another Entity has targeted us
    //public bool TargetedAsMate { get { return targetedAsMate; } }

    //protected override void Start()
    //{
    //    base.Start();
    //    StartCoroutine(MonitorMating());        
    //}

    //protected override void Update()
    //{
    //    FindMate();
    //}

    //protected virtual IEnumerator MonitorMating()
    //{
    //    yield return null;
    //}

    //protected virtual void FindMate()
    //{

    //}

    //protected virtual IEnumerator Mate()
    //{
    //    yield return null;
    //}

    //[SerializeField] private Entity whoisTargetingUs;
    //public virtual void Target(Entity whoIsTargetingUs) // Called by other entities when they want to mate with us
    //{
    //    Debug.Log(gameObject.name + " is being targeted by " + whoIsTargetingUs.gameObject.name, this);
    //    this.whoisTargetingUs = whoIsTargetingUs;
    //    targetedAsMate = true;
    //}

    //public virtual void OnBeginMate() // Called when we begin mating
    //{
    //    isMating = true;
    //}

    //public virtual void OnCompleteMate() // Called when we have finished mating
    //{
    //    matingCounter = 0.0f;
    //    shouldFindMate = false;
    //    hasTargetMate = false;
    //    reachedMateLocation = false;
    //    isMating = false;
    //    targetMate = null;
    //    targetedAsMate = false;
    //}

    //public override void Reset()
    //{
    //    base.Reset();
    //    allowMating = true;
    //    matingCounter = 0.0f;
    //    shouldFindMate = false;
    //    hasTargetMate = false;
    //    reachedMateLocation = false;
    //    isMating = false;
    //    targetMate = null;
    //    targetedAsMate = false;
    //}

    //public virtual void EnableMating() { allowMating = true; }

    //public virtual void DisableMating() { allowMating = false; }    

    [SerializeField] protected float matingCounter = 0.0f;
    protected bool shouldFindMate = false;
    protected bool hasTargetMate = false;
    protected bool isMating = false;

    protected bool isTargetedByAnother = false;

    protected Entity targetMate;

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
