using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Movement_DefaultRoaming : AI_Movement {

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
        return 0.5f;
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
        entity.GetComponent<Animator>().SetBool("isMoving", false);
        destination = Vector3.zero;
        isMoving = false;
    }

    public override void Reset()
    {
        base.Reset();
    }

    protected override void OnDestroy()
    {
        entity.GetComponent<Animator>().SetBool("isMoving", false);
        base.OnDestroy();
    }

    // ------------------------------------------------ \\

    protected override void FindDestination()
    {        
        destination = Vector3.zero;

        while (destination == Vector3.zero)
        {
            float xRange = Random.Range(-entityData.RoamingRange, entityData.RoamingRange);
            float zRange = Random.Range(-entityData.RoamingRange, entityData.RoamingRange);
            Vector3 randomDirection = Vector3.zero;
            randomDirection += transform.right * xRange;
            randomDirection += transform.forward * zRange;

            Vector3 newDestination = entity.GetHabitatOrigin + randomDirection;

            Vector3 directionFromPlanet = (Vector3.zero - newDestination).normalized;
            Ray ray = new Ray(newDestination - (directionFromPlanet * 25.0f), directionFromPlanet);

            RaycastHit[] hits;

            hits = Physics.RaycastAll(ray, 35.0f);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.tag == "TerrainSegment")
                {
                    if (entity.TerrainSegment.terrain == hit.collider.gameObject)
                    {
                        destination = hit.point;
                        break;
                    }
                }
            }
        }
    }

    protected override void MoveToDestination()
    {
        if(destination != Vector3.zero)
        {
            if (!isMoving)
            {
                isMoving = true;
                GetComponent<Entity>().GetAttributes.Update(Attribute.AttributeKey.status, "Roaming");
                entity.GetComponent<Animator>().SetBool("isMoving", true);
            }
            Vector3 moveDirection = (destination - GetComponent<Rigidbody>().position).normalized;
            GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + moveDirection * entityData.MovementSpeed * Time.deltaTime * GameTime.singleton.GameTimeMultipler);

            if(Vector3.Distance(transform.position, destination) <= 1.5f)
            {
                OnFinish();
            }
        }
    }

    public override void ClearDestination()
    {
        base.ClearDestination();
    }

}
