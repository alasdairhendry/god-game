using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabButtonHoverChild : UITabButton {

    [SerializeField] private Image targetImage;
    private Color targetImageDefaultColour;

    protected override void Start()
    {
        base.Start();
        if (targetImage == null)
            targetImage = GetComponentsInChildren<Image>()[1];

        targetImageDefaultColour = targetImage.color;
    }

    public override void SetControl(UITabControl control)
    {
        base.SetControl(control);
    }

    public override void SetActive(bool state)
    {
        isActiveTabButton = state;

        if (state)
        {
            GetComponent<Image>().color = selectedColour;
            targetImage.color = targetImageDefaultColour;
        }
        else
        {
            GetComponent<Image>().color = colour;
            targetImage.color = targetImageDefaultColour;
        }
    }

    public override void SetInteractable(bool state)
    {
        base.SetInteractable(state);
    }

    public override void InvokeRegardless()
    {
        base.InvokeRegardless();
    }

    protected override void PointerClick()
    {
        if (!interactable) return;
        onClick.Invoke();
        control.OnTabClicked(this);
    }

    protected override void PointerEnter()
    {
        if (!isActiveTabButton)
            targetImage.color = onHoverColour;
    }

    protected override void PointerExit()
    {
        if (!isActiveTabButton)
            targetImage.color = targetImageDefaultColour;
    }
}
