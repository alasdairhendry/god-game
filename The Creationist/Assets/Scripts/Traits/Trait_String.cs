using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait_String : Trait {

    protected string value;

    public Trait_String(DataType dataType, DataValue dataValue, string valueString)
    {
        this.dataType = dataType;
        this.dataValue = dataValue;
        this.valueString = valueString;
        SetValueAsString(base.valueString);
    }

    public override void SetValueAsString(string valueString)
    {
        value = valueString.ToString();
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
