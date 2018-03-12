using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

    protected Entity entity;
    protected EntityData entityData;

    // Use this for initialization
    protected virtual void Start() {
        entity = GetComponent<Entity>();
        entityData = entity.GetData;
    }

    // Update is called once per frame
    protected virtual void Update() { }

    protected virtual void FixedUpdate() { }

    public virtual void Reset() { }
}

[System.Serializable]
public class AIComponent
{
    public Object type;
}

/*
protected override void Start() { base.Start(); }

protected override void Update() { }
*/