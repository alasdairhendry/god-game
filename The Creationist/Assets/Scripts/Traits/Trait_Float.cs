using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait_Float : Trait {

    protected float value;

    public Trait_Float(DataType dataType, DataValue dataValue, string valueString)
    {
        this.dataType = dataType;
        this.dataValue = dataValue;
        this.valueString = valueString;
        SetValueAsString(base.valueString);
    }

    public override void SetValueAsString(string valueString)
    {
        value = float.Parse(valueString);
    }

    public override string GetValueAsString()
    {
        return value.ToString();
    }

    public override string GetDisplayValueAsString()
    {
        return GetValueAsString();
    }

}
