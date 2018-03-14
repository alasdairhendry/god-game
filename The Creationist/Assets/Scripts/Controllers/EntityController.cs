using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EntityController : MonoBehaviour {

    public static EntityController singleton;
    [SerializeField] private List<string> names = new List<string>();

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

   [SerializeField]  private List<EntityData> entities = new List<EntityData>();
    public List<EntityData> Entities { get { return entities; } }

    [SerializeField] private List<SelectiveData> selectiveData = new List<SelectiveData>();    

    private Queue<Action> entitiesAwaitingSpawn = new Queue<Action>();

    private GameObject gameObjectBeingPlaced;
    private EntityData dataBeingPlaced;

    private void Start()
    {
        //string json = JsonUtility.ToJson(entities[0]);        
        //File.WriteAllText("C:\\Users\\alasdair\\Desktop\\JsonData.txt", json);

        entities.Clear();

        UnityEngine.Object[] animals = Resources.LoadAll("JSON\\Entities\\Animals", typeof(TextAsset));

        foreach (UnityEngine.Object a in animals)
        {
            TextAsset b = a as TextAsset;
            //Debug.Log(a.ToString());
            EntityData data = JsonUtility.FromJson<EntityData>(a.ToString());
            entities.Add(data);
        }

        AssignEntityIDS();
        AssignEntityDataStubs();
        StartCoroutine(SpawnQueuedEntities());
        Preload();


    }

    private void AssignEntityIDS()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].SetEntityDataID = i;                     
        }
    }

    private void AssignEntityDataStubs()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].InitializeData("Entities\\" + entities[i].Species + "_Prefab", "Portraits\\" + entities[i].Species, entities[i].AverageLifetime * 372.0f);            
        }
    }

    private void Preload()
    {
        foreach (EntityData e in entities)
        {
            EntityPool.singleton.CreatePoolData(e.EntityDataID, e.PrefabStub, 100, null, null);
        }

        EntityPool.singleton.Preload();
    }

    private IEnumerator SpawnQueuedEntities()
    {
        while(true)
        {
            while (entitiesAwaitingSpawn.Count <= 0)
                yield return null;

            Action action = entitiesAwaitingSpawn.Dequeue();
            action();
            yield return null;
        }
    }

    private void Update()
    {
        if (InteractionState.singleton.CurrentState == InteractionState.State.PlacingEntity)
        {
            if(Input.GetMouseButtonDown(0))
            {
                PlaceEntity();
            }
        }
    }
        
    public void BeginPlacingEntity(EntityData data)
    {
        SpawnEntityPrefab(data.EntityDataID);
        InteractionState.singleton.SetPlacingEntity(true);
        dataBeingPlaced = data;
    }

    public void StopPlacingEntity()
    {
        InteractionState.singleton.SetPlacingEntity(false);
        dataBeingPlaced = null;
        Destroy(gameObjectBeingPlaced);
    }

    private void SpawnEntityPrefab(int id)
    {
        GameObject go = Instantiate(Resources.Load(entities[id].PrefabStub)) as GameObject;
        go.AddComponent<EntityPickupControl>();
        gameObjectBeingPlaced = go;
        go.GetComponent<Entity>().enabled = false;
        go.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }

    private void PlaceEntity()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits;

        hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag == "TerrainSegment")
            {
                Vector3 direction = (Vector3.zero - hit.point).normalized;
                CreateEntity(dataBeingPlaced, hit.point, TerrainEntity.singleton.FindSegment(hit.collider.gameObject));
                //CreateEntity(dataBeingPlaced.EntityDataID, hit.point, direction, TerrainEntity.singleton.FindSegment(hit.collider.gameObject), Mathf.Lerp(1.5f, 20.0f, Mathf.InverseLerp(FindObjectOfType<CameraOrbitMotion>().ZoomLimit, 1.0f, FindObjectOfType<CameraOrbitMotion>().ZoomLevel)));                

                Destroy(gameObjectBeingPlaced);
                gameObjectBeingPlaced = null;

                InteractionState.singleton.SetPlacingEntity(false);
                break;
            }
        }
    }

    public void CreateEntity(EntityData entityData, Vector3 worldPosition, TerrainEntity.TerrainSegment segment)
    {
        entitiesAwaitingSpawn.Enqueue(() =>
        {
            GameObject go = EntityPool.singleton.Instantiate(entityData.EntityDataID);
            Vector3 direction = (Vector3.zero - worldPosition).normalized;
            go.transform.position = worldPosition - (direction * Mathf.Lerp(1.5f, 20.0f, Mathf.InverseLerp(FindObjectOfType<CameraOrbitMotion>().ZoomLimit, 1.0f, FindObjectOfType<CameraOrbitMotion>().ZoomLevel)));
            go.transform.parent = GravityAttractor.singleton.transform.Find("Entities");

            go.GetComponent<Entity>().Initialize(entityData, segment);
            go.GetComponent<Entity>().SetHabitatOrigin = worldPosition;

            if(entityData.IsSelective && !entityData.IsUnlocked)
            {
                entityData.Unlock();
                NotificationController.singleton.Add(new Notification(entityData.PortraitStub, "New Species Evolved!", "The " + entityData.Species + " species has evolved through selective breeding!").SetAction_RotateToGameObject(go));
            }

        });
    }

    //public void CreateEntity(int id, Vector3 terrainNodePosition, Vector3 direction, TerrainEntity.TerrainSegment spawnSegment)
    //{
    //    entitiesAwaitingSpawn.Enqueue(() => {
    //        GameObject go = EntityPool.singleton.Instantiate(id);            
    //        go.transform.position = terrainNodePosition - (direction * 20.0f);
    //        go.transform.parent = GravityAttractor.singleton.transform.Find("Entities");

    //        NotificationController.singleton.Add(new Notification(dataBeingPlaced.PortraitStub, "Placed Entity!", "You have placed a baby " + dataBeingPlaced.InfantName + " in the world.").SetAction_RotateToGameObject(go));

    //        go.GetComponent<Entity>().Initialize(entities[id], spawnSegment);
    //        go.GetComponent<Entity>().SetHabitatOrigin = terrainNodePosition;
    //    });
    //}

    //public void CreateEntity(int id, Vector3 terrainNodePosition, Vector3 direction, TerrainEntity.TerrainSegment spawnSegment, float offsetFromPlanet)
    //{
    //    entitiesAwaitingSpawn.Enqueue(() =>
    //    {            
    //        GameObject go = EntityPool.singleton.Instantiate(id);
    //        go.transform.position = terrainNodePosition - (direction * offsetFromPlanet);
    //        go.transform.parent = GravityAttractor.singleton.transform.Find("Entities");

    //        NotificationController.singleton.Add(new Notification(dataBeingPlaced.PortraitStub, "Placed Entity!", "You have placed a baby " + dataBeingPlaced.InfantName + " in the world.").SetAction_RotateToGameObject(go));

    //        go.GetComponent<Entity>().Initialize(entities[id], spawnSegment);
    //        go.GetComponent<Entity>().SetHabitatOrigin = terrainNodePosition;
    //    });
    //}

    public string GetRandomAnimalName()
    {
        return names[UnityEngine.Random.Range(0, names.Count)];
    }

    public EntityData ReturnEntityDataByName(string name)
    {
        foreach (EntityData d in entities)
        {
            if (d.Species == name)
                return d;
        }

        return null;
    }

    //public void Breed(Entity primary, Entity secondary)
    //{
    //    if(primary.GetData.Name == secondary.GetData.Name)  // Bred the same species
    //    {
    //        Vector3 d = (Vector3.zero - primary.transform.position).normalized;
    //        CreateEntity(primary.GetData.EntityDataID, primary.transform.position, d, primary.TerrainSegment, 5.0f);
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
    //    EntityData entityToCreate = ReturnEntityDataByName(chosenData.ResultingEntityName);
    //    Vector3 direction = (Vector3.zero - primary.transform.position).normalized;
    //    CreateEntity(entityToCreate.EntityDataID, primary.transform.position, direction, primary.TerrainSegment, 5.0f);
    //    NotificationController.singleton.Add(new Notification(entityToCreate.PortraitStub, "A Newborn!", "A baby " + entityToCreate.InfantName + " has been born."));
    //}

    //public void BreedAtPoint(Entity primary, Entity secondary, Vector3 worldPosition)
    //{
    //    if (primary.GetData.Name == secondary.GetData.Name)  // Bred the same species
    //    {
    //        Vector3 d = (Vector3.zero - worldPosition).normalized;
    //        CreateEntity(primary.GetData.EntityDataID, worldPosition, d, primary.TerrainSegment, 5.0f);
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
    //    CreateEntity(ReturnEntityDataByName(chosenData.ResultingEntityName).EntityDataID, worldPosition, direction, primary.TerrainSegment, 5.0f);
    //}
}
