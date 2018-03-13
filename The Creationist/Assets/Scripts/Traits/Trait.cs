using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public class Trait {

    public enum DataType { STRING, INT, FLOAT }

    [SerializeField] protected DataType dataType = DataType.STRING;
    public DataType GetDataType { get { return dataType; } }

    public enum DataValue {
        [Description("Size")] size,
        [Description("Preferred Habitat")] preferredBiome,
        [Description("Biome Forest")] biomeForest,
        [Description("Biome Plains")] biomePlains,
        [Description("Biome Tundra")] biomeTundra
    }

    [SerializeField] protected DataValue dataValue;
    public DataValue GetDataValue { get { return dataValue; } }

    protected string valueString = "";

    public Trait ()
    {
        
    }

    public virtual void SetValueAsString(string valueString)
    {

    }

    public virtual string GetValueAsString()
    {
        return valueString;
    }

    public virtual string GetDisplayValueAsString()
    {
        return GetValueAsString();
    }

}
