using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UITabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected UnityEvent onClick;
    protected UITabControl control;

    [SerializeField] protected bool interactable = true;
    [SerializeField] protected Color colour = Color.white;
    [SerializeField] protected Color onHoverColour = new Color(1, 1, 1, 128.0f / 255.0f);
    [SerializeField] protected Color selectedColour = new Color(1, 1, 1, 100.0f / 255.0f);
    [SerializeField] protected Color disabledColour = new Color(0.3f, 0.3f, 0.3f, 1.0f);

    protected bool isActiveTabButton = false;

    protected virtual void Start()
    {
        GetComponent<Image>().color = colour;
    }

    public virtual void SetControl(UITabControl control)
    {
        this.control = control;
    }

    public virtual void SetActive(bool state)
    {
        isActiveTabButton = state;

        if(state)
            GetComponent<Image>().color = selectedColour;
        else
            GetComponent<Image>().color = colour;
    }

    public virtual void SetInteractable(bool state)
    {
        interactable = state;

        if(state)
        {
            if(isActiveTabButton)
            {
                GetComponent<Image>().color = selectedColour;
            }
            else
            {
                GetComponent<Image>().color = colour;
            }
        }
        else
        {
            GetComponent<Image>().color = disabledColour;

        }
    }

    public virtual void InvokeRegardless()
    {
        onClick.Invoke();
    }

    protected virtual void PointerClick()
    {
        if (!interactable) return;
        onClick.Invoke();
        control.OnTabClicked(this);
    }

    protected virtual void PointerEnter()
    {
        if (!isActiveTabButton)
            GetComponent<Image>().color = onHoverColour;
    }

    protected virtual void PointerExit()
    {
        if (!isActiveTabButton)
            GetComponent<Image>().color = colour;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        PointerClick();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        PointerExit();
    }
}
