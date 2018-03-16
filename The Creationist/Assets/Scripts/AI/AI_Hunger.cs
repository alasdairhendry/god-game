using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Hunger : AI {

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
        Debug.Log("Hunger: " + entity.GetAttributes.FindAttributeByKey(Attribute.AttributeKey.hunger).FloatValue);
        Debug.Log("Hunger Priority: " + Mathf.Lerp(1.0f, 0.0f, Mathf.InverseLerp(0.3f, 1.0f, entity.GetAttributes.FindAttributeByKey(Attribute.AttributeKey.hunger).FloatValue)));
        return Mathf.Lerp(1.0f, 0.0f, Mathf.InverseLerp(0.3f, 1.0f, entity.GetAttributes.FindAttributeByKey(Attribute.AttributeKey.hunger).FloatValue));
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
}
