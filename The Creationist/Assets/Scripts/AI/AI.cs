using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
    
    protected Entity entity;
    protected EntityData entityData;
    protected AIController aiController;
    protected bool isActiveAI = false;      // Are we the active AI for this component?

    // Use this for initialization
    protected virtual void Start() {
        entity = GetComponent<Entity>();
        entityData = entity.GetData;
        aiController = GetComponent<AIController>();
        aiController.AddAIComponent(this);
    }

    // Update is called once per frame
    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }

    public virtual float GetPriority() { return 0.0f; }

    public virtual void Begin() { isActiveAI = true; aiController.SetActiveAI(this); OnStart(); }

    public virtual void Stop() { isActiveAI = false; OnFinish(); }

    protected virtual void OnStart() { }

    protected virtual void OnFinish() { isActiveAI = false; aiController.OnActiveAIFinished(this); }

    public virtual void Reset() { }

    protected virtual void OnDestroy()
    {
        OnFinish();
        GetComponent<AIController>().RemoveAIComponent(this);
    }
}

[System.Serializable]
public class AIComponent
{
    public Object type;
}

/*
 
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

*/
