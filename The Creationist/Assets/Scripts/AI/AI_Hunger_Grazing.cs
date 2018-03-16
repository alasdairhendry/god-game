using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Hunger_Grazing : AI_Hunger {

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
        StartCoroutine(Eat());
    }

    private IEnumerator Eat()
    {
        entity.GetAttributes.Update(Attribute.AttributeKey.status, "Grazing");
        entity.GetComponent<Animator>().SetBool("isEating", true);

        float currentFullness = entity.GetAttributes.FindAttributeByKey(Attribute.AttributeKey.hunger).FloatValue;
        float targetFullness = Mathf.Clamp(Random.Range(0.80f, 1.0f), currentFullness, 1.0f);

        float feedingTime = (targetFullness - currentFullness) * 25.0f;

        yield return StartCoroutine(GameTime.singleton.Yield(feedingTime));

        entity.GetAttributes.Update(Attribute.AttributeKey.hunger, targetFullness);

        entity.GetComponent<Animator>().SetBool("isEating", false);
        entity.GetAttributes.Update(Attribute.AttributeKey.status, "Idle");
        OnFinish();
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
