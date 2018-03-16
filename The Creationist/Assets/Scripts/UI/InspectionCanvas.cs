using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InspectionCanvas : MonoBehaviour {
    
    [SerializeField] private RectTransform inspectionPanel;

    [SerializeField] private InputField nameInputField;

    [SerializeField] private Transform detailsTab_PairsPanel;
    [SerializeField] private Transform attributesTab_PairsPanel;

    [SerializeField] private GameObject attributeText_Prefab;

    [SerializeField] private GameObject tab_Details;
    [SerializeField] private GameObject tab_Attributes;    

    private GameObject target;
    private List<Attribute> targetAttributes = new List<Attribute>();
    List<Attribute.AttributeKey> detailsTabKeys = new List<Attribute.AttributeKey>();

	// Use this for initialization
	void Start () {
        Close();
        HideAllTabs();
        detailsTabKeys = new List<Attribute.AttributeKey>()
        {
             Attribute.AttributeKey.species,
             Attribute.AttributeKey.age,
             Attribute.AttributeKey.hunger,
             Attribute.AttributeKey.status,
             Attribute.AttributeKey.preferredBiome,
             Attribute.AttributeKey.diet
        };
	}
	
	// Update is called once per frame
	void Update () {
        MoveInspectionPanel();
        UpdateTab_Details();
        UpdateTab_Attributes();
	}

    public void Open(GameObject target)
    {
        if(inspectionPanel.gameObject.activeSelf)
        {
            Close();
        }

        inspectionPanel.gameObject.SetActive(true);
        this.target = target;
        this.targetAttributes = target.GetComponent<Entity>().GetAttributes;
        ShowTab_Details();
    }

    public void Close()
    {
        inspectionPanel.gameObject.SetActive(false);
    }

    private void HideAllTabs()
    {
        tab_Details.SetActive(false);
        tab_Attributes.SetActive(false);
    }

    public void ShowTab_Details()
    {
        if (target == null) return;
        if (targetAttributes.Count <= 0) return;

        HideAllTabs();
        tab_Details.SetActive(true);        

        for (int i = 0; i < detailsTab_PairsPanel.childCount; i++)
        {
            Destroy(detailsTab_PairsPanel.GetChild(i).gameObject);
        }

        List<Attribute> attributesToDisplay = new List<Attribute>();

        if (targetAttributes.FindAttributeByKey(Attribute.AttributeKey.species) != null)
        {
            attributesToDisplay.Add(targetAttributes.FindAttributeByKey(Attribute.AttributeKey.species));
        }

        if (targetAttributes.FindAttributeByKey(Attribute.AttributeKey.age) != null)
        {
            attributesToDisplay.Add(targetAttributes.FindAttributeByKey(Attribute.AttributeKey.age));
        }

        if (targetAttributes.FindAttributeByKey(Attribute.AttributeKey.hunger) != null)
        {
            attributesToDisplay.Add(targetAttributes.FindAttributeByKey(Attribute.AttributeKey.hunger));
        }

        if (targetAttributes.FindAttributeByKey(Attribute.AttributeKey.diet) != null)
        {
            attributesToDisplay.Add(targetAttributes.FindAttributeByKey(Attribute.AttributeKey.diet));
        }

        if (targetAttributes.FindAttributeByKey(Attribute.AttributeKey.preferredBiome) != null)
        {
            attributesToDisplay.Add(targetAttributes.FindAttributeByKey(Attribute.AttributeKey.preferredBiome));
        }

        if (targetAttributes.FindAttributeByKey(Attribute.AttributeKey.status) != null)
        {
            attributesToDisplay.Add(targetAttributes.FindAttributeByKey(Attribute.AttributeKey.status));
        }

        foreach (Attribute attribute in attributesToDisplay)
        {
            if (attribute.Key == Attribute.AttributeKey.name)
            {
                nameInputField.text = attribute.GetValue();
                continue;
            }
            GameObject go = Instantiate(Resources.Load("UI\\TextAttribute_Prefab")) as GameObject;
            go.transform.parent = detailsTab_PairsPanel;
            go.transform.Find("Name").GetComponent<Text>().text = attribute.DisplayName.ToString();
            go.transform.Find("Value").GetComponent<Text>().text = attribute.GetValue();
        }
    }

    public void UpdateTab_Details()
    {
        if (target == null) return;
        if (targetAttributes.Count <= 0) return;

        if (!tab_Details.activeSelf) return;

        if (EventSystem.current.currentSelectedGameObject != nameInputField.gameObject)
            nameInputField.text = targetAttributes.FindValueByName("Name");

        for (int i = 0; i < detailsTab_PairsPanel.childCount; i++)
        {
            string name = detailsTab_PairsPanel.GetChild(i).Find("Name").GetComponent<Text>().text;

            if (targetAttributes.FindValueByName(name) == "Attribute Not Found")
            {
                Debug.LogError("Error: Attribute Not Found", this);
                continue;
            }

            detailsTab_PairsPanel.GetChild(i).Find("Value").GetComponent<Text>().text = targetAttributes.FindValueByName(name);
        }
    }

    public void ShowTab_Attributes()
    {
        if (target == null) return;
        if (targetAttributes.Count <= 0) return;

        HideAllTabs();
        tab_Attributes.SetActive(true);        

        for (int i = 0; i < attributesTab_PairsPanel.childCount; i++)
        {
            Destroy(attributesTab_PairsPanel.GetChild(i).gameObject);
        }

        List<Attribute> attributesToDisplay = new List<Attribute>();

        foreach (Attribute attribute in targetAttributes)
        {
            //if (attribute.Key != Attribute.AttributeKey.species)
            //{
            //    if (attribute.Key != Attribute.AttributeKey.age)
            //    {
            //        if (attribute.Key != Attribute.AttributeKey.status)
            //        {
            //            if (attribute.Key != Attribute.AttributeKey.preferredBiome)
            //            {
            //                if (attribute.Key != Attribute.AttributeKey.hunger)
            //                {
            //                    attributesToDisplay.Add(attribute);
            //                }
            //            }
            //        }
            //    }
            //}

            if(detailsTabKeys.Contains(attribute.Key))
            {
                continue;
            }

            attributesToDisplay.Add(attribute);
        }

        foreach (Attribute attribute in attributesToDisplay)
        {
            if (attribute.Key == Attribute.AttributeKey.name)
            {
                nameInputField.text = attribute.GetValue();
                continue;
            }
            GameObject go = Instantiate(Resources.Load("UI\\TextAttribute_Prefab")) as GameObject;
            go.transform.parent = attributesTab_PairsPanel;
            go.transform.Find("Name").GetComponent<Text>().text = attribute.DisplayName.ToString();
            go.transform.Find("Value").GetComponent<Text>().text = attribute.GetValue();
        }
    }

    private void UpdateTab_Attributes()
    {
        if (target == null) return;
        if (targetAttributes.Count <= 0) return;

        if (!tab_Attributes.activeSelf) return;

        if (EventSystem.current.currentSelectedGameObject != nameInputField.gameObject)
            nameInputField.text = targetAttributes.FindValueByName("Name");

        for (int i = 0; i < attributesTab_PairsPanel.childCount; i++)
        {
            string name = attributesTab_PairsPanel.GetChild(i).Find("Name").GetComponent<Text>().text;

            if (targetAttributes.FindValueByName(name) == "Attribute Not Found")
            {
                Debug.LogError("Error: Attribute Not Found", this);
                continue;
            }

            attributesTab_PairsPanel.GetChild(i).Find("Value").GetComponent<Text>().text = targetAttributes.FindValueByName(name);
        }
    }

    private void MoveInspectionPanel()
    {
        if (!inspectionPanel.gameObject.activeSelf) return;        
        if (!target) return;

        // The position in screen pixels of the target
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);

        Vector2 screenToUIPosition = new Vector2(0, 0);
        screenToUIPosition.x = Mathf.Lerp(0, GetComponent<RectTransform>().sizeDelta.x, screenPosition.x / Screen.width ) - 32;
        screenToUIPosition.y = Mathf.Lerp(0, GetComponent<RectTransform>().sizeDelta.y, screenPosition.y / Screen.height) - 32;
        screenToUIPosition.x = Mathf.Clamp(screenToUIPosition.x, 448.0f + 48.0f, 1920.0f - 48.0f);
        screenToUIPosition.y = Mathf.Clamp(screenToUIPosition.y, 256.0f + 48.0f, 1080.0f - 48.0f);

        inspectionPanel.anchoredPosition = Vector2.Lerp(inspectionPanel.anchoredPosition, screenToUIPosition, 15 * Time.deltaTime);
    }

    public void UpdateEntityName()
    {
        targetAttributes.Update(Attribute.AttributeKey.name, nameInputField.text);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
