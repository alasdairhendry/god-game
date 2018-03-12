using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InspectionCanvas : MonoBehaviour {

    [SerializeField] private RectTransform inspectionPanel;
    [SerializeField] private InputField nameInputField;
    [SerializeField] private Transform attributePairPanel;
    [SerializeField] private GameObject attributeText_Prefab;
    private GameObject targetGO;
    private List<Attribute> targetAttributes = new List<Attribute>();
    private bool follow = false;

	// Use this for initialization
	void Start () {
        HideInspectionPanel();	
	}
	
	// Update is called once per frame
	void Update () {
        MoveInspectionPanel();
        UpdateAttributeDetails();
	}

    public void ShowInspectionPanel(bool follow, GameObject targetInspectable)
    {
        inspectionPanel.gameObject.SetActive(true);
        this.follow = follow;
        this.targetGO = targetInspectable;
        this.targetAttributes = targetInspectable.GetComponent<IInspectable>().GetAttributes();
        CreateAttributeDetails();
    }

    public void HideInspectionPanel()
    {
        inspectionPanel.gameObject.SetActive(false);
    }

    private void MoveInspectionPanel()
    {
        if (!inspectionPanel.gameObject.activeSelf) return;
        if (!follow) return;
        if (!targetGO) return;

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(targetGO.transform.position);
        screenPosition -= inspectionPanel.sizeDelta / 2;
        Vector2 uiPosition = Vector2.zero;
        uiPosition.x = Mathf.Lerp(16, (GetComponent<RectTransform>().sizeDelta.x - inspectionPanel.sizeDelta.x - 16), screenPosition.x / Screen.width);
        uiPosition.y = Mathf.Lerp(16, (GetComponent<RectTransform>().sizeDelta.y - inspectionPanel.sizeDelta.y - 16), screenPosition.y / Screen.height);
        //uiPosition.x -= inspectionPanel.sizeDelta.x / 2 + 16;
        //uiPosition.y -= inspectionPanel.sizeDelta.y / 2 + 16;
        //uiPosition.x = Mathf.Clamp(uiPosition.x, 16, GetComponent<RectTransform>().sizeDelta.x - inspectionPanel.sizeDelta.x - 16);
        //uiPosition.y = Mathf.Clamp(uiPosition.y, 16, GetComponent<RectTransform>().sizeDelta.y - inspectionPanel.sizeDelta.y - 16);

        inspectionPanel.anchoredPosition = Vector2.Lerp(inspectionPanel.anchoredPosition, uiPosition, 15 * Time.deltaTime);
    }

    private void CreateAttributeDetails()
    {
        for (int i = 0; i < attributePairPanel.childCount; i++)
        {
            Destroy(attributePairPanel.GetChild(i).gameObject);
        }        

        foreach (Attribute a in targetAttributes)
        {            
            if(a.Key == Attribute.AttributeKey.name)
            {
                nameInputField.text = a.GetValue();
                continue;
            }
            GameObject go = Instantiate(Resources.Load("UI\\TextAttribute_Prefab")) as GameObject;
            go.transform.parent = attributePairPanel;
            go.transform.Find("Name").GetComponent<Text>().text = a.DisplayName.ToString();
            go.transform.Find("Value").GetComponent<Text>().text = a.GetValue();
        }        
    }

    private void UpdateAttributeDetails()
    {
        if (!inspectionPanel.gameObject.activeSelf) return;
        if (!follow) return;
        if (!targetGO) return;

        if (EventSystem.current.currentSelectedGameObject != nameInputField.gameObject)
            nameInputField.text = targetAttributes.FindValueByName("Name");

        for (int i = 0; i < attributePairPanel.childCount; i++)
        {
            string name = attributePairPanel.GetChild(i).Find("Name").GetComponent<Text>().text;

            if (targetAttributes.FindValueByName(name) == "Attribute Not Found")
            {
                Debug.LogError("Error: Attribute Not Found", this);
                continue;
            }

            attributePairPanel.GetChild(i).Find("Value").GetComponent<Text>().text = targetAttributes.FindValueByName(name);
        }
    }

    public void UpdateEntityName()
    {
        targetAttributes.Update(Attribute.AttributeKey.name, nameInputField.text);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
