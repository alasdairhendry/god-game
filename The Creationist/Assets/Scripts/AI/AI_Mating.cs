﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Mating : AI {

    protected bool allowMating = true;

    protected float matingCounter = 0.0f;

    protected bool shouldFindMate = false;  // Should we look for an eligible mate?
    public bool ShouldFindMate { get { return shouldFindMate; } set { shouldFindMate = value; } }

    protected bool hasTargetMate = false;   // We have targeted an entity
    public bool HasTargetMate { get { return hasTargetMate; } }

    protected bool reachedMateLocation = false; // Are we next to our mate?
    public bool ReachedMateLocation { get { return reachedMateLocation; } }

    protected bool isMating = false;    // Are we currently mating? 
    public bool IsMating { get { return isMating; } set { isMating = value; } }

    protected Entity targetMate;    // We have targeted this entity to mate with

    protected bool targetedAsMate = false;  // Another Entity has targeted us
    public bool TargetedAsMate { get { return targetedAsMate; } }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(MonitorMating());        
    }

    protected override void Update()
    {
        FindMate();
    }

    protected virtual IEnumerator MonitorMating()
    {
        yield return null;
    }

    protected virtual void FindMate()
    {

    }

    protected virtual IEnumerator Mate()
    {
        yield return null;
    }

    [SerializeField] private Entity whoisTargetingUs;
    public virtual void Target(Entity whoIsTargetingUs) // Called by other entities when they want to mate with us
    {
        Debug.Log(gameObject.name + " is being targeted by " + whoIsTargetingUs.gameObject.name, this);
        this.whoisTargetingUs = whoIsTargetingUs;
        targetedAsMate = true;
    }

    public virtual void OnBeginMate() // Called when we begin mating
    {
        isMating = true;
    }

    public virtual void OnCompleteMate() // Called when we have finished mating
    {
        matingCounter = 0.0f;
        shouldFindMate = false;
        hasTargetMate = false;
        reachedMateLocation = false;
        isMating = false;
        targetMate = null;
        targetedAsMate = false;
    }

    public override void Reset()
    {
        base.Reset();
        allowMating = true;
        matingCounter = 0.0f;
        shouldFindMate = false;
        hasTargetMate = false;
        reachedMateLocation = false;
        isMating = false;
        targetMate = null;
        targetedAsMate = false;
    }

    public virtual void EnableMating() { allowMating = true; }

    public virtual void DisableMating() { allowMating = false; }    
}