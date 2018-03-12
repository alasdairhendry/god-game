using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Movement_Herd : AI_Movement {

    private Entity targetEntity;
    public Entity TargetEntity { get { return targetEntity; } }
    private bool followingSpecificEntity = false;
    private Vector2 targetEntityOffset = Vector3.zero;
    private float maxDestinationOffset = 4.5f;
    private float minDestinationOffset = 2.5f;    

    protected override void Start()
    {
        base.Start();
        StartCoroutine(MonitorRoaming());        
    }

    protected override void Update() {
        if(isMoving && allowMovement)
        {
            GetComponent<Animator>().SetBool("isMoving", true);
        }
        else GetComponent<Animator>().SetBool("isMoving", false);
    }    

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override IEnumerator MonitorRoaming()
    {
        while(true)
        {
            yield return new WaitForSeconds(base.roamingCheckTick / GameTime.singleton.GameTimeMultipler);
            
            while (!base.allowMovement) yield return null;
            while (base.isMoving) yield return null;
            while (base.entity.IsPickedUp) yield return null;
            while (base.entity.AIMating.IsMating) yield return null;
            while (base.entity.AIMating.ShouldFindMate) yield return null;
            while (base.entity.AIMating.HasTargetMate) yield return null;
            while (base.entity.AIMating.TargetedAsMate) yield return null;

            float chanceToRoam = UnityEngine.Random.value;

            if (chanceToRoam > 0.75f)
            {
                List<Entity> entitiesInTerrainNode = base.entity.TerrainSegment.ReturnEntitiesByType(entityData.Name);                

                // There is no entities of this type in this node, so just wonder to a random location
                if(entitiesInTerrainNode.Count <= 1)
                {
                    Debug.Log("Only me in this node", this);
                    SetRandomDestination();

                }
                else    // There is entities of this type in this terrain node, so lets group up with them
                {
                    SetEntityDestination(entitiesInTerrainNode);
                }
            }
        }
    }

    private void SetRandomDestination()
    {
        base.destination = TerrainEntity.singleton.ReturnRandomSegmentLocation(base.entity.TerrainSegment);

        Vector3 newDestination = base.destination;
        Vector3 direction = (Vector3.zero - newDestination).normalized;

        Ray ray = new Ray(newDestination - (direction * 5), direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.collider.gameObject.tag == "TerrainSegment")
            {
                             
            }
            else
            {
                Debug.LogError("SetRandomDestination: NOT HITTING TERRAIN");
            }
        }
        else
        {
            Debug.LogError("SetRandomDestination: NOT HITTING TERRAIN");
        }

        isMoving = true;
        targetEntity = null;
        followingSpecificEntity = false;
    }

    private void SetEntityDestination(List<Entity> entitiesInTerrainNode)
    {
        float distance = Mathf.Infinity;
        Entity nearestEntity = base.entity;

        foreach (Entity e in entitiesInTerrainNode)
        {
            if (e == base.entity) continue;
            if (e.GetComponent<AI_Movement_Herd>().TargetEntity == base.entity) continue;

            if (Vector3.Distance(transform.position, e.transform.position) < distance)
            {
                nearestEntity = e;
                distance = Vector3.Distance(transform.position, e.transform.position);
            }
        }

        if (nearestEntity != null && nearestEntity != base.entity)
        {
            targetEntity = nearestEntity;
            followingSpecificEntity = true;
            isMoving = true;
            base.destination = targetEntity.transform.position;

            int positiveNegative = Random.Range(0, 2);
            if (positiveNegative == 0)
                targetEntityOffset = new Vector2(Random.Range(minDestinationOffset, maxDestinationOffset), Random.Range(minDestinationOffset, maxDestinationOffset));
            else targetEntityOffset = new Vector2(Random.Range(-maxDestinationOffset, -minDestinationOffset), Random.Range(-maxDestinationOffset, -minDestinationOffset));
        }
        else
        {
            Debug.Log("Couldnt follow an entity, roaming randomly");
            SetRandomDestination();
        }
    }

    protected override void MoveToDestination()
    {
        if (!base.allowMovement) return;
        if (!base.isMoving) return;
        if (base.entity.AIMating.ShouldFindMate) return;
        if (base.entity.AIMating.HasTargetMate) return;
        if (base.entity.AIMating.TargetedAsMate) return;
        if (base.entity.AIMating.IsMating) return;
        if (base.entity.IsPickedUp) return;                

        if (followingSpecificEntity)
        {
            if (targetEntity == null) { isMoving = false; }

            Vector3 randomDirectionOffset = Vector3.zero;
            randomDirectionOffset += targetEntity.transform.right * targetEntityOffset.x;
            randomDirectionOffset += targetEntity.transform.forward * targetEntityOffset.y;

            Vector3 newDestination = targetEntity.transform.position + randomDirectionOffset;
            Vector3 direction = (Vector3.zero - newDestination).normalized;

            Ray ray = new Ray(newDestination - (direction * 5), direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.collider.gameObject.tag == "TerrainSegment")
                {                    
                    destination = newDestination;
                }
            }

            Vector3 AnewDestination = base.destination;
            Vector3 Adirection = (Vector3.zero - AnewDestination).normalized;

            Ray Aray = new Ray(AnewDestination - (Adirection * 5), Adirection);
            RaycastHit Ahit;

            if (Physics.Raycast(Aray, out Ahit, 100.0f))
            {
                if (Ahit.collider.gameObject.tag == "TerrainSegment")
                {
                    
                }
                else
                {
                    Debug.LogError("SetEntityDestination: NOT HITTING TERRAIN");
                }
            }
            else
            {
                Debug.LogError("SetEntityDestination: NOT HITTING TERRAIN");
            }

            Vector3 moveDirection = (destination - GetComponent<Rigidbody>().position).normalized;
            GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + moveDirection * entityData.MovementSpeed * Time.deltaTime * GameTime.singleton.GameTimeMultipler);
        }
        else
        {
            Vector3 moveDirection = (destination - GetComponent<Rigidbody>().position).normalized;
            GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + moveDirection * entityData.MovementSpeed * Time.deltaTime * GameTime.singleton.GameTimeMultipler);
        }    
        
        if(Vector3.Distance(transform.position, destination)<= 1.0f)
        {
            OnReachedDestination();
        }
    }    

    protected override void OnReachedDestination()
    {
        isMoving = false;
        followingSpecificEntity = false;
        targetEntity = null;
        destination = Vector3.zero;
    }

    public override void ClearDestination()
    {
        isMoving = false;
        followingSpecificEntity = false;
        targetEntity = null;
        destination = Vector3.zero;
    }

    public override void Reset()
    {
        base.Reset();
        allowMovement = true;
        isMoving = false;
        destination = Vector3.zero;
        targetEntity = null;
        followingSpecificEntity = false;
        targetEntityOffset = Vector3.zero;
        maxDestinationOffset = 4.5f;
        minDestinationOffset = 2.5f;
    }

    public override void EnableMovement() { allowMovement = true; }

    public override void DisableMovement() { allowMovement = true; }

}
