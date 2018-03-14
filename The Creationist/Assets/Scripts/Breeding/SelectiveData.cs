using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectiveData {

    [SerializeField] private string resultingEntityName = "";
    public string ResultingEntityName { get { return resultingEntityName; } }

    [SerializeField] private string breedingEntityName = "";
    public string BreedingEntityName { get { return breedingEntityName; } }

    //[SerializeField] private string secondaryEntityName = "";
    //public string SecondaryEntityName { get { return secondaryEntityName; } }

    [SerializeField] private List<BreedingPartnerRequirement> breedingRequirements = new List<BreedingPartnerRequirement>();
    //[SerializeField] private List<BreedingPartnerRequirement> secondaryBreedingRequirements = new List<BreedingPartnerRequirement>();       

    public bool Check(Entity entity)
    {
        //Entity primaryEntity;
        //Entity secondaryEntity;

        //Debug.Log("Checking Resulting Entity " + resultingEntityName);

        //if(entityA.GetData.Name == breedingEntityName && entityB.GetData.Name == secondaryEntityName)
        //{
        //    primaryEntity = entityA;
        //    secondaryEntity = entityB;
        //}
        //else if (entityA.GetData.Name == secondaryEntityName && entityB.GetData.Name == breedingEntityName)
        //{
        //    primaryEntity = entityB;
        //    secondaryEntity = entityA;
        //}
        //else
        //{
        //    // The name dont match, these two species cannot interbreed
        //    return false;
        //}

        for (int i = 0; i < breedingRequirements.Count; i++)
        {
            //Debug.Log("Checking Primary Requirements " + i);
            BreedingPartnerRequirement requirement = breedingRequirements[i];
            Attribute attribute = entity.GetAttributes.FindAttributeByKey(requirement.AttributeKey);
            Debug.Log("Checking Attribute " + entity.GetData.Species + " - " + attribute.Key + " - " + attribute.Type + " - " + attribute.GetValue());

            //if (attribute.Type != requirement.AttributeType) { Debug.LogError("Invalid Attribute Match " + breedingEntityName + " + " + secondaryEntityName + ": Result = " + resultingEntityName); return false; } // The attribute and requirement types do not match, an error made in the inspector, so notify ourselves so we can correct it 
            if (attribute == null) { Debug.LogError("Mate Does not have required attribute"); return false; }   // The mate does not have a required Attribute, therefore this breed cannot continue

            // This mate has the required attribute, so we need to check the value
            if (!CheckValue(requirement, attribute))
                return false;
        }

        //for (int i = 0; i < secondaryBreedingRequirements.Count; i++)
        //{
        //    Debug.Log("Checking Secondary Requirements " + i);
        //    BreedingPartnerRequirement requirement = secondaryBreedingRequirements[i];
        //    Attribute attribute = secondaryEntity.GetAttributes.FindAttributeByKey(requirement.AttributeKey);

        //    if (attribute.Type != requirement.AttributeType) { Debug.LogError("Invalid Attribute Match " + breedingEntityName + " + " + secondaryEntityName + ": Result = " + resultingEntityName); return false; } // The attribute and requirement types do not match, an error made in the inspector, so notify ourselves so we can correct it 
        //    if (attribute == null) return false;    // The mate does not have a required Attribute, therefore this breed cannot continue

        //    // This mate has the required attribute, so we need to check the value
        //    if (!CheckValue(requirement, attribute))
        //        return false;
        //}

        //return false;

        //foreach (BreedingPartnerRequirement requirement in primaryBreedingRequirements)
        //{
        //    Attribute attribute = primaryEntity.GetAttributes.FindAttributeByKey(requirement.AttributeKey);

        //    if (attribute.Type != requirement.AttributeType) { Debug.LogError("Invalid Attribute Match " + primaryEntityName + " + " + secondaryEntityName + ": Result = " + resultingEntityName); return false; } // The attribute and requirement types do not match, an error made in the inspector, so notify ourselves so we can correct it 
        //    if (attribute == null) return false;    // The mate does not have a required Attribute, therefore this breed cannot continue

        //    // This mate has the required attribute, so we need to check the value
        //    if (CheckValue(requirement, attribute))
        //        continue;
        //    else return false;
        //}

        //foreach (BreedingPartnerRequirement requirement in secondaryBreedingRequirements)
        //{
        //    Attribute attribute = secondaryEntity.GetAttributes.FindAttributeByKey(requirement.AttributeKey);

        //    if (attribute.Type != requirement.AttributeType) { Debug.LogError("Invalid Attribute Match " + primaryEntityName + " + " + secondaryEntityName + ": Result = " + resultingEntityName); return false; } // The attribute and requirement types do not match, an error made in the inspector, so notify ourselves so we can correct it 
        //    if (attribute == null) return false;    // The mate does not have a required Attribute, therefore this breed cannot continue

        //    // This mate has the required attribute, so we need to check the value
        //    if (CheckValue(requirement, attribute))
        //        continue;
        //    else return false;
        //}

        // We have made it this far, so all the attribute requirements have been met. Allow species to be created
        return true;
    }

    private bool CheckValue(BreedingPartnerRequirement requirement, Attribute attribute)
    {
        if (requirement.AttributeType == Attribute.AttributeType.STRING)
        {
            return CheckFloatValue(requirement, attribute);
        }
        else if (requirement.AttributeType == Attribute.AttributeType.INT)
        {
            return CheckFloatValue(requirement, attribute);
        }
        else if (requirement.AttributeType == Attribute.AttributeType.FLOAT)
        {
            return CheckFloatValue(requirement, attribute);
        }
        else return false;
    }

    private bool CheckStringValue(BreedingPartnerRequirement requirement, Attribute attribute)
    {
        string value = attribute.GetValue();
        string reqValue = requirement.RequiredValueString;

        switch(requirement.AttributeOperator)
        {
            case BreedingPartnerRequirement.AttrOperator.GreaterThanOrEqual:
                Debug.LogError("Invalid Operator " + breedingEntityName + ": Result = " + resultingEntityName);
                return false;

            case BreedingPartnerRequirement.AttrOperator.GreaterThan:
                Debug.LogError("Invalid Operator " + breedingEntityName + ": Result = " + resultingEntityName);
                return false;

            case BreedingPartnerRequirement.AttrOperator.EqualTo:
                return (value == reqValue);                

            case BreedingPartnerRequirement.AttrOperator.NotEqualTo:
                return (value == reqValue);

            case BreedingPartnerRequirement.AttrOperator.LessThan:
                Debug.LogError("Invalid Operator " + breedingEntityName + ": Result = " + resultingEntityName);
                return false;

            case BreedingPartnerRequirement.AttrOperator.LessThanOrEqual:
                Debug.LogError("Invalid Operator " + breedingEntityName + ": Result = " + resultingEntityName);
                return false;

            default: return false;
        }
    }

    private bool CheckIntValue(BreedingPartnerRequirement requirement, Attribute attribute)
    {
        int value = int.Parse(attribute.GetValue());
        int reqValue = int.Parse(requirement.RequiredValueString);

        switch (requirement.AttributeOperator)
        {
            case BreedingPartnerRequirement.AttrOperator.GreaterThanOrEqual:
                return (value >= reqValue);                

            case BreedingPartnerRequirement.AttrOperator.GreaterThan:
                return (value > reqValue);

            case BreedingPartnerRequirement.AttrOperator.EqualTo:
                return (value == reqValue);

            case BreedingPartnerRequirement.AttrOperator.NotEqualTo:
                return (value != reqValue);

            case BreedingPartnerRequirement.AttrOperator.LessThan:
                return (value < reqValue);

            case BreedingPartnerRequirement.AttrOperator.LessThanOrEqual:
                return (value <= reqValue);

            default: return false;
        }
    }

    private bool CheckFloatValue(BreedingPartnerRequirement requirement, Attribute attribute)
    {
        float value = float.Parse(attribute.GetValue());
        float reqValue = float.Parse(requirement.RequiredValueString);

        switch (requirement.AttributeOperator)
        {
            case BreedingPartnerRequirement.AttrOperator.GreaterThanOrEqual:
                return (value >= reqValue);

            case BreedingPartnerRequirement.AttrOperator.GreaterThan:
                return (value > reqValue);

            case BreedingPartnerRequirement.AttrOperator.EqualTo:
                return (value == reqValue);

            case BreedingPartnerRequirement.AttrOperator.NotEqualTo:
                return (value != reqValue);

            case BreedingPartnerRequirement.AttrOperator.LessThan:
                return (value < reqValue);

            case BreedingPartnerRequirement.AttrOperator.LessThanOrEqual:
                Debug.Log(value <= reqValue);
                return (value <= reqValue);

            default: return false;
        }
    }
}

[System.Serializable]
public class BreedingPartnerRequirement
{
    public enum AttrOperator { GreaterThanOrEqual, GreaterThan, EqualTo, NotEqualTo, LessThan, LessThanOrEqual }

    [SerializeField] private Attribute.AttributeKey attributeKey;
    public Attribute.AttributeKey AttributeKey { get { return attributeKey; } }

    [SerializeField] private Attribute.AttributeType attributeType;
    public Attribute.AttributeType AttributeType { get { return attributeType; } }

    [SerializeField] private AttrOperator attributeOperator = AttrOperator.EqualTo;
    public AttrOperator AttributeOperator { get { return attributeOperator; } }

    [SerializeField] private string requiredValueString = "";
    public string RequiredValueString { get { return requiredValueString; } }
}
