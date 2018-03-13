using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public class Attribute {

    public enum AttributeKey
    {
        [Description("Name")] name,
        [Description("Age")] age,
        [Description("Species")] species,
        [Description("Entity Type")] entityType,
        [Description("Status")] status,
        [Description("Size")] size,
        [Description("Biome Plains")] biomePlains,
        [Description("Biome Forest")] biomeForest,
        [Description("Biome Grasslands")] biomeGrasslands,
        [Description("Biome Tundra")] biomeTundra,
        [Description("Preferred Biome")] preferredBiomePlains,
        [Description("Preferred Biome")] preferredBiomeForest,
        [Description("Preferred Biome")] preferredBiomeGrasslands,
        [Description("Preferred Biome")] preferredBiomeTundra,
        [Description("Biome Plains")] matingBiomePlains,
        [Description("Biome Forest")] matingBiomeForest,
        [Description("Biome Grasslands")] matingBiomeGrasslands,
        [Description("Biome Tundra")] matingBiomeTundra,
        [Description("Movement Speed")] movementSpeedModifer,
        [Description("Temperment")] temperment
    }

    [SerializeField] private AttributeKey key;
    public AttributeKey Key { get { return key; } }

    public enum AttributeType { STRING, INT, FLOAT }
    [SerializeField] private AttributeType type;
    public AttributeType Type { get { return type; } }

    private string displayName = "";
    public string DisplayName { get { return displayName; } }
    [SerializeField] private string value = "";

    [HideInInspector] private string stringValue = "";
    public string StringValue { get { return stringValue; } }

    [HideInInspector] private int intValue = -1;
    public int IntValue { get { return intValue; } }

    [HideInInspector] private float floatValue = 0.0f;
    public float FloatValue { get { return floatValue; } }

    public Attribute(AttributeKey key, string value)
    {
        this.key = key;
        stringValue = value;
        type = AttributeType.STRING;
        this.value = value.ToString();
        this.displayName = CustomHelper.GetDescription(key);
    }

    public Attribute(AttributeKey key, int value)
    {
        this.key = key;
        intValue = value;
        type = AttributeType.INT;
        this.value = value.ToString();
        this.displayName = CustomHelper.GetDescription(key);
    }

    public Attribute(AttributeKey key, float value)
    {
        this.key = key;
        floatValue = value;
        type = AttributeType.FLOAT;
        this.value = value.ToString();
        this.displayName = CustomHelper.GetDescription(key);
    }

    public virtual void ChangeAttribute(string value)
    {
        if (type != AttributeType.STRING) return;
        stringValue = value;
        this.value = value.ToString();
    }

    public virtual void ChangeAttribute(int value)
    {
        if (type != AttributeType.INT) return;
        intValue = value;
        this.value = value.ToString();
    }

    public virtual void ChangeAttribute(float value)
    {
        if (type != AttributeType.FLOAT) return;
        floatValue = value;
        this.value = value.ToString();
    }      

    public virtual string GetValue()
    {
        if (key == AttributeKey.age)
            return GameTime.singleton.DaysToTime(float.Parse(value));

        return value;
    }

    [System.Serializable]
    public class AttributeJSONData
    {
        public string key;
        public string type;
        public string value;

        public AttributeJSONData(string key, string type, string value)
        {
            this.key = key;
            this.type = type;
            this.value = value;
        }
    }
}
