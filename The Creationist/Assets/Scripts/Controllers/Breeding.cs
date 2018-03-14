using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breeding : MonoBehaviour {

    public static Breeding singleton;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

    [SerializeField] private List<SelectiveData> selectiveData = new List<SelectiveData>();
    private EntityController entityController;

    private void Start()
    {
        entityController = EntityController.singleton;
    }

    public void Breed(Entity primaryEntity, Entity secondaryEntity, Vector3 spawnPosition)
    {
        List<EntityData> eligibleOffspring = new List<EntityData>();

        foreach (SelectiveData data in selectiveData)
        {
            EntityData dat = entityController.ReturnEntityDataByName(data.ResultingEntityName);

            if(dat.IsSelective)
            {
                if(!dat.IsUnlocked)
                {
                    if (data.Check(primaryEntity))
                        eligibleOffspring.Add(dat);
                }
            }
        }

        EntityData entityToCreate;

        if (eligibleOffspring.Count > 0)
        {
            entityToCreate = eligibleOffspring[Random.Range(0, eligibleOffspring.Count)];
        }
        else
        {
            entityToCreate = primaryEntity.GetData;
        }

        entityController.CreateEntity(entityToCreate, spawnPosition, primaryEntity.TerrainSegment);
    }

    //public bool CheckMateMatch(Entity primary, Entity secondary) // Check to see if two entities can procreate
    //{
    //    if (primary.GetData.Name == secondary.GetData.Name) return true;    // Always allow same species mating

    //    // The two entities are of a different species.. Check to see if they can selectively breed
    //    foreach (SelectiveData d in selectiveData)
    //    {
    //        if ((primary.GetData.Name == d.PrimaryEntityName && secondary.GetData.Name == d.SecondaryEntityName) || (secondary.GetData.Name == d.PrimaryEntityName && primary.GetData.Name == d.SecondaryEntityName))
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    //public void Breed(Entity primary, Entity secondary)
    //{
    //    if (primary.GetData.Name == secondary.GetData.Name)  // Bred the same species
    //    {
    //        Vector3 d = (Vector3.zero - primary.transform.position).normalized;
    //        entityController.CreateEntity(primary.GetData.EntityDataID, primary.transform.position, d, primary.TerrainSegment, 5.0f);
    //        return;
    //    }

    //    List<SelectiveData> availableBreeds = new List<SelectiveData>();

    //    foreach (SelectiveData d in selectiveData)
    //    {
    //        if ((primary.GetData.Name == d.PrimaryEntityName && secondary.GetData.Name == d.SecondaryEntityName) || (secondary.GetData.Name == d.PrimaryEntityName && primary.GetData.Name == d.SecondaryEntityName))
    //        {
    //            if (d.Check(primary, secondary))
    //                availableBreeds.Add(d);
    //            else return;
    //        }
    //    }

    //    // Breed the selective species
    //    if (availableBreeds.Count < 0)
    //        return;

    //    SelectiveData chosenData = availableBreeds[UnityEngine.Random.Range(0, availableBreeds.Count)];
    //    EntityData entityToCreate = entityController.ReturnEntityDataByName(chosenData.ResultingEntityName);
    //    Vector3 direction = (Vector3.zero - primary.transform.position).normalized;
    //    entityController.CreateEntity(entityToCreate.EntityDataID, primary.transform.position, direction, primary.TerrainSegment, 5.0f);
    //    NotificationController.singleton.Add(new Notification(entityToCreate.PortraitStub, "A Newborn!", "A baby " + entityToCreate.InfantName + " has been born."));
    //}

    //public void BreedAtPoint(Entity primary, Entity secondary, Vector3 worldPosition)
    //{
    //    if (primary.GetData.Name == secondary.GetData.Name)  // Bred the same species
    //    {
    //        Vector3 d = (Vector3.zero - worldPosition).normalized;
    //        entityController.CreateEntity(primary.GetData.EntityDataID, worldPosition, d, primary.TerrainSegment, 5.0f);
    //        return;
    //    }

    //    List<SelectiveData> availableBreeds = new List<SelectiveData>();

    //    foreach (SelectiveData d in selectiveData)
    //    {
    //        if ((primary.GetData.Name == d.PrimaryEntityName && secondary.GetData.Name == d.SecondaryEntityName) || (secondary.GetData.Name == d.PrimaryEntityName && primary.GetData.Name == d.SecondaryEntityName))
    //        {
    //            if (d.Check(primary, secondary))
    //                availableBreeds.Add(d);
    //            else return;
    //        }
    //    }

    //    // Breed the selective species
    //    if (availableBreeds.Count < 0)
    //        return;

    //    SelectiveData chosenData = availableBreeds[UnityEngine.Random.Range(0, availableBreeds.Count)];
    //    Vector3 direction = (Vector3.zero - worldPosition).normalized;
    //    entityController.CreateEntity(entityController.ReturnEntityDataByName(chosenData.ResultingEntityName).EntityDataID, worldPosition, direction, primary.TerrainSegment, 5.0f);
    //}

}
