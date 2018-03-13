using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Movement : AI {

    //protected float roamingCheckTick = 1.0f;  // How long to wait inbetween roaming checks
    //protected bool allowMovement = true;

    //protected bool isMoving = false;
    //protected Vector3 destination = Vector3.zero;

    //protected override void Start() { base.Start(); }    

    //protected override void Update() { }

    //protected override void FixedUpdate()
    //{
    //    MoveToDestination();
    //}

    //protected virtual IEnumerator MonitorRoaming() { yield return null; }    

    //protected virtual void MoveToDestination() { }

    //protected virtual void OnReachedDestination() { }

    //public virtual void ClearDestination() { }

    //public virtual void EnableMovement() { allowMovement = true; }

    //public virtual void DisableMovement() { allowMovement = true; }

    //public void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(destination, 1f);
    //}

    protected bool isMoving = false;
    protected Vector3 destination = Vector3.zero;

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

        MoveToDestination();
    }

    public override float GetPriority()
    {
        return base.GetPriority();
    }

    //public override void Begin()
    //{
    //    base.Begin();
    //}

    //public override void Stop()
    //{
    //    base.Stop();
    //}

    protected override void OnStart()
    {
        base.OnStart();

        FindDestination();
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

    // ------------------------------------------------ \\

    protected virtual void FindDestination()
    {

    }

    protected virtual void MoveToDestination() { }    

    public virtual void ClearDestination()
    {
        OnFinish();
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(destination, 1.5f);
    }
}

/*
    protected override void Start() { base.Start(); }

    protected override void Update() { }

    protected override IEnumerator MonitorRoaming() { yield return null; }

    protected override void SetDestination(Vector3 location) { }

    protected override void MoveToDestination() { }

    public override void EnableMovement() { allowMovement = true; }

    public override void DisableMovement() { allowMovement = true; }
*/
