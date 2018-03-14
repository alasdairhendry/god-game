using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private bool displayUnlocked = true;
    [SerializeField] private bool displayLocked = false;
    [SerializeField] private bool displaySelective = false;

    [SerializeField] private Toggle showSelective_Toggle;
    [SerializeField] private Toggle showLocked_Toggle;

    EntityData.EntityType currentTypeDisplayed = EntityData.EntityType.Mammal;

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
                Show(EntityData.EntityType.Mammal);
            }
        }
    }

    public void ShowStore_Mammal()
    {
        currentTypeDisplayed = EntityData.EntityType.Mammal;
        Show(currentTypeDisplayed);
    }

    public void ShowStore_Reptile()
    {
        currentTypeDisplayed = EntityData.EntityType.Reptile;
        Show(currentTypeDisplayed);
    }

    public void ShowStore_Bird()
    {
        currentTypeDisplayed = EntityData.EntityType.Bird;
        Show(currentTypeDisplayed);
    }

    public void ShowStore_Tree()
    {
        currentTypeDisplayed = EntityData.EntityType.Tree;
        Show(currentTypeDisplayed);
    }

    public void ShowStore_Bush()
    {
        currentTypeDisplayed = EntityData.EntityType.Bush;
        Show(currentTypeDisplayed);
    }

    public void ShowStore_Flower()
    {
        currentTypeDisplayed = EntityData.EntityType.Flower;
        Show(currentTypeDisplayed);
    }

    public void ShowStore_Rock()
    {
        currentTypeDisplayed = EntityData.EntityType.Rock;
        Show(currentTypeDisplayed);
    }

    public void Refresh()
    {
        Show(currentTypeDisplayed);
    }

    private void Show(EntityData.EntityType type)
    {
        storePanel.SetActive(true);

        displaySelective = showSelective_Toggle.isOn;
        displayLocked = showLocked_Toggle.isOn;

        DisplayEntities(type);
        DisplaySelected(EntityController.singleton.Entities[0]);
    }

    public void Hide()
    {
        storePanel.SetActive(false);
    }

    private void DisplayEntities(EntityData.EntityType type)
    {
        for (int i = 0; i < options_BodyPanel.transform.childCount; i++)
        {
            Destroy(options_BodyPanel.transform.GetChild(i).gameObject);
        }

        GameObject firstPanel = Instantiate(Resources.Load("UI\\StoreHorizontalPanel_Prefab")) as GameObject;
        firstPanel.transform.parent = options_BodyPanel.transform;
        int counter = 0;

        List<EntityData> dataToShow = new List<EntityData>();

        foreach (EntityData e in EntityController.singleton.Entities)
        {
            if (e.GetEntityType != type) continue;
            if (displayUnlocked)
            {
                if(e.IsUnlocked)
                {
                    dataToShow.Add(e);
                    continue;
                }
            }
            if(displayLocked)
            {
                if(!e.IsUnlocked)
                {
                    dataToShow.Add(e);
                    continue;
                }
            }
            if ( displaySelective)
            {
                if(e.IsSelective)
                {
                    dataToShow.Add(e);
                    continue;
                }
            }
        }

        List<EntityData> orderedDataToShow = dataToShow.OrderByDescending(o => o.IsUnlocked).ToList();

        foreach (EntityData e in orderedDataToShow)
        {
            GameObject go = Instantiate(Resources.Load("UI\\StoreEntityButton_Prefab")) as GameObject;
            go.transform.parent = firstPanel.transform;
            go.transform.Find("Portrait_Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(e.PortraitStub);
            go.transform.Find("Header_Panel").GetComponentInChildren<Text>().text = e.Species;            

            if (!e.IsUnlocked)
            {
                go.GetComponent<Image>().color = Color.gray;
            }


            if (!e.IsUnlocked)
            {
                if (DEBUG_AllowAllPurchases)
                {
                    go.GetComponent<Button>().onClick.AddListener(() => { EntityController.singleton.BeginPlacingEntity(e); Hide(); });
                    go.transform.Find("Cost_Panel").GetComponent<Image>().color = Color.green;
                    go.transform.Find("Cost_Panel").GetComponentInChildren<Text>().text = "Debug Purchase";
                }
                else
                {                    
                    go.transform.Find("Cost_Panel").GetComponent<Image>().color = Color.red;
                    go.transform.Find("Cost_Panel").GetComponentInChildren<Text>().text = "Locked";
                }
            }
            else
            {
                go.GetComponent<Button>().onClick.AddListener(() => { EntityController.singleton.BeginPlacingEntity(e); Hide(); });
                go.transform.Find("Cost_Panel").GetComponent<Image>().color = Color.green;
                go.transform.Find("Cost_Panel").GetComponentInChildren<Text>().text = "$" + e.CostToBuy.ToString("0.00");
            }


            counter++;

            if (counter >= 3)
            {
                firstPanel = Instantiate(Resources.Load("UI\\StoreHorizontalPanel_Prefab")) as GameObject;
                firstPanel.transform.parent = options_BodyPanel.transform;
                counter = 0;
            }
        }

    }

    private void DisplaySelected(EntityData data)
    {
        return;

        for (int i = 0; i < display_BodyPanel.transform.childCount; i++)
        {
            Destroy(display_BodyPanel.transform.GetChild(i).gameObject);
        }
        display_PortraitImage.sprite = Resources.Load<Sprite>(data.PortraitStub);
        display_EntityNameText.text = data.Species;

        GameObject attributeOne = Instantiate(Resources.Load("UI\\TextAttribute_Prefab")) as GameObject;
        attributeOne.transform.parent = display_BodyPanel.transform;
        attributeOne.transform.Find("Name").GetComponent<Text>().text = "Species";
        attributeOne.transform.Find("Value").GetComponent<Text>().text = data.Species;

        GameObject attributeTwo = Instantiate(Resources.Load("UI\\TextAttribute_Prefab")) as GameObject;
        attributeTwo.transform.parent = display_BodyPanel.transform;
        attributeTwo.transform.Find("Name").GetComponent<Text>().text = "Life Expectancy";
        attributeTwo.transform.Find("Value").GetComponent<Text>().text = GameTime.singleton.DaysToTime(data.AverageLifetime);

        foreach (Attribute.AttributeJSONData a in data.InitialAttributes)
        {
            if (a.key == Attribute.AttributeKey.biomeForest.ToString())
            {
                GameObject att = Instantiate(Resources.Load("UI\\TextAttribute_Prefab")) as GameObject;
                att.transform.parent = display_BodyPanel.transform;
                att.transform.Find("Name").GetComponent<Text>().text = CustomHelper.GetDescription(Attribute.AttributeKey.biomeForest);
                att.transform.Find("Value").GetComponent<Text>().text = float.Parse(a.value).ToString("0.00"); ;
            }
            if (a.key == Attribute.AttributeKey.biomeGrasslands.ToString())
            {
                GameObject att = Instantiate(Resources.Load("UI\\TextAttribute_Prefab")) as GameObject;
                att.transform.parent = display_BodyPanel.transform;
                att.transform.Find("Name").GetComponent<Text>().text = CustomHelper.GetDescription(Attribute.AttributeKey.biomeGrasslands);
                att.transform.Find("Value").GetComponent<Text>().text = float.Parse(a.value).ToString("0.00");
            }
            if (a.key == Attribute.AttributeKey.biomeTundra.ToString())
            {
                GameObject att = Instantiate(Resources.Load("UI\\TextAttribute_Prefab")) as GameObject;
                att.transform.parent = display_BodyPanel.transform;
                att.transform.Find("Name").GetComponent<Text>().text = CustomHelper.GetDescription(Attribute.AttributeKey.biomeTundra);
                att.transform.Find("Value").GetComponent<Text>().text = float.Parse(a.value).ToString("0.00");
            }
        }

        // Preferred Biomes
        List<string> preferredBiomes = new List<string>();
        foreach (Attribute.AttributeJSONData a in data.InitialAttributes)
        {
            if(a.key == Attribute.AttributeKey.preferredBiome.ToString())
            {
                preferredBiomes.Add(a.value);
            }
        }

        string preferredBiomesString = preferredBiomes.Commafy<string>();
        if(preferredBiomesString != "List Empty")
        {
            GameObject attributeThree = Instantiate(Resources.Load("UI\\TextAttribute_Prefab")) as GameObject;
            attributeThree.transform.parent = display_BodyPanel.transform;
            attributeThree.transform.Find("Name").GetComponent<Text>().text = preferredBiomes.Count.PluraliseString("Preferred Biome", "Preferred Biomes");
            attributeThree.transform.Find("Value").GetComponent<Text>().text = preferredBiomesString;
        }               



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
