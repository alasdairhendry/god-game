using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractionCanvas : MonoBehaviour {

    private int segments = 2;

    [SerializeField] private Transform RadialMenuOrigin_Panel;

    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private GameObject closePrefab;

    [SerializeField] private float segmentDimensions = 0.0f;
    [SerializeField] private float segmentOriginOffset = 0.0f;
    [SerializeField] private float closeButtonScaleModifier = 0.5f;
    [SerializeField] private bool closeButton = false;
    [SerializeField] private bool spawnOnMouse = false;

    [SerializeField] private bool holdToDisplay = false;

    private List<RadialOptionMenu> activeOptions = new List<RadialOptionMenu>();
    private int hoveredIndex = 0;

    private float defaultOffset = 1.5f;

    // Update is called once per frame
    void Update()
    {
        if (!holdToDisplay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<Entity>() != null)
                    {
                        OpenRadialMenu(hit.collider.gameObject.GetComponent<Entity>().RadialOptions);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideRadialMenu();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<Entity>() != null)
                    {
                        OpenRadialMenu(hit.collider.gameObject.GetComponent<Entity>().RadialOptions);
                    }
                }
            }
            else if(Input.GetMouseButtonUp(0))
            {
                if(hoveredIndex == -1)
                {
                    HideRadialMenu();
                    return;
                }
                else
                {                    
                    if (activeOptions.Count > 0)
                        activeOptions[hoveredIndex].OnClick();
                    hoveredIndex = -1;                    
                    HideRadialMenu();
                    return;
                }
            }
        }
    }

    private void OpenRadialMenu(List<RadialOptionMenu> options)
    {
        RadialMenuOrigin_Panel.gameObject.SetActive(true);
        activeOptions = options;

        for (int i = 0; i < RadialMenuOrigin_Panel.childCount; i++)
        {
            Destroy(RadialMenuOrigin_Panel.GetChild(i).gameObject);
        }

        if(spawnOnMouse)
        {
            RadialMenuOrigin_Panel.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
        }

        segments = options.Count;

        if (segments == 0)
        {
            Debug.LogError("Segments cannot equal 0.", this);
            return;
        }

        // Spawn close button
        if (closeButton)
        {
            GameObject closeButton = Instantiate(closePrefab);
            closeButton.transform.parent = RadialMenuOrigin_Panel;
            closeButton.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            closeButton.GetComponent<RectTransform>().sizeDelta = new Vector2(segmentDimensions, segmentDimensions) * closeButtonScaleModifier;
            closeButton.GetComponent<RectTransform>().localScale = Vector3.one;
        }

        if (segments == 1)
        {
            Vector2 direction = new Vector2(0.0f, 1.0f);

            SpawnSegment(direction, options[0], 0);
        }
        else if (segments == 2)
        {
            Vector2 direction = new Vector2(0.0f, 1.0f);
            SpawnSegment(direction, options[0], 0);

            direction = new Vector2(0.0f, -1.0f);
            SpawnSegment(direction, options[1], 1);
        }
        else
        {
            float angleIncrement = 360.0f / (float)segments;
            float angle = 0.0f;

            for (int i = 0; i < segments; i++)
            {
                Vector2 direction = new Vector2((float)Mathf.Sin(Mathf.Deg2Rad * angle), (float)Mathf.Cos(Mathf.Deg2Rad * angle)).normalized;
                SpawnSegment(direction, options[i], i);

                angle += angleIncrement;
            }
        }
    }    

    private void HideRadialMenu()
    {
        RadialMenuOrigin_Panel.gameObject.SetActive(false);
    }

    private void SpawnSegment(Vector2 direction, RadialOptionMenu option, int index)
    {
        GameObject go = Instantiate(segmentPrefab);
        go.transform.parent = RadialMenuOrigin_Panel;
        go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        go.GetComponent<RectTransform>().localScale = Vector3.one;
        go.GetComponent<RectTransform>().anchoredPosition += direction * ((segmentDimensions + segmentOriginOffset) * defaultOffset);

        if (option.AutoDeselect)
            go.GetComponent<Button>().onClick.AddListener(() => { option.OnClick(); EventSystem.current.SetSelectedGameObject(null); HideRadialMenu(); });
        else
            go.GetComponent<Button>().onClick.AddListener(() => { option.OnClick(); HideRadialMenu(); });

        if(holdToDisplay)
        {
            go.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry onEnter = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            onEnter.callback.AddListener((e) => {
                hoveredIndex = index;
            });

            EventTrigger.Entry onExit = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            onExit.callback.AddListener((e) => {
                hoveredIndex = -1;
            });

            go.GetComponent<EventTrigger>().triggers.Add(onEnter);
            go.GetComponent<EventTrigger>().triggers.Add(onExit);
        }

        if (option.Icon != null)
            go.transform.GetChild(0).GetComponent<Image>().sprite = option.Icon;
    }
}