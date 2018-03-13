using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityData {

    [Header("Base")]
    [SerializeField] private string name = "New Entity";
    public string Name { get { return name; } }

    [SerializeField] private int entityDataID = 0;
    public int EntityDataID { get { return entityDataID; } }
    public int SetEntityDataID { set { entityDataID = value; } }

    [SerializeField] private string infantName = "New Infant";
    public string InfantName { get { return infantName; } }

    [SerializeField] private string description = "New Description";
    public string Description { get { return description; } }

    [SerializeField] private int costToBuy = 50;
    public int CostToBuy { get { return costToBuy; } }

    [SerializeField] private int rewardForBreed = 25;
    public int RewardForBreed { get { return rewardForBreed; } }

    [SerializeField] [Tooltip("The average time this entity lives for (years)")] private float averageLifetime = 15;    // Years
    public float AverageLifetime { get { return averageLifetime; } }

    [SerializeField] [Tooltip("The speed at which this entity moves")] private float movementSpeed = 3;
    public float MovementSpeed { get { return movementSpeed; } }

    [SerializeField] private List<Attribute.AttributeJSONData> initialAttributes = new List<Attribute.AttributeJSONData>();
    public List<Attribute.AttributeJSONData> InitialAttributes { get { return initialAttributes; } }

    [SerializeField] private List<string> initialAI = new List<string>();
    public List<string> InitialAI { get { return initialAI; } }

    [Header("Roaming")]
    [SerializeField] protected float roamingRange = 15.0f;
    public float RoamingRange { get { return roamingRange; } }

    [Header("Mating")]
    [SerializeField] [Tooltip("The length between each mating session (days)")] private float averageMateDelay = 3;      // Days
    public float AverageMateDelay { get { return averageMateDelay; } }

    [SerializeField] [Tooltip("The chance of this entity mates when its eligible (0, 1)")] [Range(0, 1)] private float mateChance = 3;    // Between 0 and 1
    public float MateChance { get { return mateChance; } }

    [SerializeField] [Tooltip("The success chance of each mating session (0, 1)")] [Range(0, 1)] private float fertility = 3;     // Between 0 and 1
    public float Fertility { get { return fertility; } }

    [SerializeField] [Tooltip("The length this entity will travel to mate (meters)")] private float matingRange = 3;      // Meters
    public float MatingRange { get { return matingRange; } }

    [Header("Data")]
    [SerializeField] private string prefabStub = "New Stub";
    public string PrefabStub { get { return prefabStub; } }

    [SerializeField] private string portraitStub = "New Stub";
    public string PortraitStub { get { return portraitStub; } }     

    [Header("Selective Breeding")]
    [SerializeField] private bool isSelective = false;
    public bool IsSelective { get { return isSelective; } }

    [SerializeField] private bool isUnlocked = false;
    public bool IsUnlocked { get { return isUnlocked; } }

    public void InitializeData(string prefabStub, string portraitStub, float averageLifetime)
    {
        this.prefabStub = prefabStub;
        this.portraitStub = portraitStub;
        this.averageLifetime = averageLifetime;
    }

    public void Unlock()
    {
        if(!isUnlocked)
        {
            isUnlocked = true;
            if(IsSelective)
            {
                Debug.Log("A new species has been discovered! It has been called " + name);
            }
            else
            {
                Debug.Log("A species has just been unlocked! " + name);
            }
        }
    }

    //public EntityData(EntityData data, GameObject activeGameObject, TerrainEntity.TerrainSegment spawnSegment)
    //{
    //    // Base
    //    this.entityDataID = data.EntityDataID;
    //    this.name = data.name;
    //    this.infantName = data.infantName;
    //    this.description = data.description;
    //    this.averageLifetime = data.averageLifetime;
    //    this.movementSpeed = data.movementSpeed;

    //    // Mating
    //    this.averageMateDelay = data.averageMateDelay;
    //    this.mateChance = data.mateChance;
    //    this.fertility = data.fertility;
    //    this.matingRange = data.matingRange;

    //    // Data
    //    this.portraitStub = data.portraitStub;

    //    this.activeGameObject = activeGameObject;
    //    activeGameObject.GetComponent<Entity>().Initialize(this, spawnSegment);
    //}
}
