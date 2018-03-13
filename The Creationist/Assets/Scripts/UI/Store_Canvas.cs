using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store_Canvas : MonoBehaviour {

    public static Store_Canvas singleton;
    [SerializeField] private bool DEBUG_AllowAllPurchases = true;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

    [SerializeField] private GameObject storePanel;

    [SerializeField] private Image display_PortraitImage;
    [SerializeField] private Text display_EntityNameText;
    [SerializeField] private GameObject display_BodyPanel;
    [SerializeField] private Button display_PurchaseButton;
    [SerializeField] private GameObject options_BodyPanel;

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            EntityController.singleton.StopPlacingEntity();
            if (storePanel.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }

    public void Show()
    {
        storePanel.SetActive(true);
        DisplayEntities();
        DisplaySelected(EntityController.singleton.Entities[0]);
    }

    public void Hide()
    {
        storePanel.SetActive(false);
    }

    private void DisplayEntities()
    {
        for (int i = 0; i < options_BodyPanel.transform.childCount; i++)
        {
            Destroy(options_BodyPanel.transform.GetChild(i).gameObject);
        }

        GameObject firstPanel = Instantiate(Resources.Load("UI\\StoreHorizontalPanel_Prefab")) as GameObject;
        firstPanel.transform.parent = options_BodyPanel.transform;
        int counter = 0;
        foreach (EntityData e in EntityController.singleton.Entities)
        {
            GameObject go = Instantiate(Resources.Load("UI\\StoreEntityButton_Prefab")) as GameObject;
            go.transform.parent = firstPanel.transform;            
            go.transform.Find("Portrait_Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(e.PortraitStub);
            go.transform.Find("Header_Panel").GetComponentInChildren<Text>().text = e.Name;

            counter++;

            go.GetComponent<Button>().onClick.AddListener(() => { DisplaySelected(e); });

            if(counter >= 3)
            {
                firstPanel = Instantiate(Resources.Load("UI\\StoreHorizontalPanel_Prefab")) as GameObject;
                firstPanel.transform.parent = options_BodyPanel.transform;
                counter = 0;
            }
        }
    }

    private void DisplaySelected(EntityData data)
    {
        for (int i = 0; i < display_BodyPanel.transform.childCount; i++)
        {
            Destroy(display_BodyPanel.transform.GetChild(i).gameObject);
        }
        display_PortraitImage.sprite = Resources.Load<Sprite>(data.PortraitStub);
        display_EntityNameText.text = data.Name;

        GameObject entityType = Instantiate(Resources.Load("UI\\TextAttribute_Prefab")) as GameObject;
        entityType.transform.parent = display_BodyPanel.transform;
        entityType.transform.Find("Name").GetComponent<Text>().text = "Animalia";
        entityType.transform.Find("Value").GetComponent<Text>().text = data.Name;

        GameObject lifeTime = Instantiate(Resources.Load("UI\\TextAttribute_Prefab")) as GameObject;
        lifeTime.transform.parent = display_BodyPanel.transform;
        lifeTime.transform.Find("Name").GetComponent<Text>().text = "Life Expectancy";
        lifeTime.transform.Find("Value").GetComponent<Text>().text = GameTime.singleton.DaysToTime(data.AverageLifetime);

        display_PurchaseButton.onClick.RemoveAllListeners();
        if (!data.IsUnlocked)
        {
            if (DEBUG_AllowAllPurchases)
            {
                display_PurchaseButton.onClick.AddListener(() => { EntityController.singleton.BeginPlacingEntity(data); Hide(); });
                display_PurchaseButton.GetComponentInChildren<Text>().text = "Purchase";
            }
            else
            {
                display_PurchaseButton.GetComponentInChildren<Text>().text = "Locked";
            }
        }
        else
        {
            display_PurchaseButton.onClick.AddListener(() => { EntityController.singleton.BeginPlacingEntity(data); Hide(); });
            display_PurchaseButton.GetComponentInChildren<Text>().text = "Purchase";
        }
    }
}
