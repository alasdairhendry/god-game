using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityData {

    [SerializeField] private string species = "New Entity";
    public string Species { get { return species; } }

    public enum EntityType { Mammal = 0, Reptile = 1, Bird = 2, Tree = 3, Bush = 4, Flower = 5, Rock = 6 }
    [SerializeField] private EntityType entityType = EntityType.Mammal;
    public EntityType GetEntityType { get { return entityType; } }   

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

    [SerializeField] private float hungerDecreaseModifier = 0.001f;
    public float HungerDecreaseModifer { get { return hungerDecreaseModifier; } }

    [SerializeField] private List<Attribute.AttributeJSONData> initialAttributes = new List<Attribute.AttributeJSONData>();
    public List<Attribute.AttributeJSONData> InitialAttributes { get { return initialAttributes; } }

    [SerializeField] private List<string> initialAI = new List<string>();
    public List<string> InitialAI { get { return initialAI; } }

    [SerializeField] protected float roamingRange = 15.0f;
    public float RoamingRange { get { return roamingRange; } }

    [SerializeField] [Tooltip("The length between each mating session (days)")] private float averageMateDelay = 3;      // Days
    public float AverageMateDelay { get { return averageMateDelay; } }

    [SerializeField] [Tooltip("The chance of this entity mates when its eligible (0, 1)")] [Range(0, 1)] private float mateChance = 3;    // Between 0 and 1
    public float MateChance { get { return mateChance; } }

    [SerializeField] [Tooltip("The success chance of each mating session (0, 1)")] [Range(0, 1)] private float fertility = 3;     // Between 0 and 1
    public float Fertility { get { return fertility; } }

    [SerializeField] [Tooltip("The length this entity will travel to mate (meters)")] private float matingRange = 3;      // Meters
    public float MatingRange { get { return matingRange; } }
    
    [SerializeField] private string prefabStub = "New Stub";
    public string PrefabStub { get { return prefabStub; } }

    [SerializeField] private string portraitStub = "New Stub";
    public string PortraitStub { get { return portraitStub; } }     
    
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
        if (!isUnlocked)
        {
            isUnlocked = true;
            if (IsSelective)
            {
                Debug.Log("A new species has been discovered! It has been called " + species);
            }
            else
            {
                Debug.Log("A species has just been unlocked! " + species);
            }
        }
        else Debug.LogError(Species + " is already unlocked!");    
    }
}
