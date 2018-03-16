using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabControl : MonoBehaviour {

    [SerializeField] private List<UITabButton> tabButtons = new List<UITabButton>();
    [SerializeField] private bool resetOnEnable = true;
    [SerializeField] private int initialTabIndex = 0;

    private void Start()
    {
        UITabButton[] but = GetComponentsInChildren<UITabButton>();
        //Debug.Log(but.Length);
        foreach (UITabButton b in but)
        {
            tabButtons.Add(b);
            b.SetControl(this);
        }

        ResetTab();
    }

    private void OnEnable()
    {
        //Debug.Log("OnEnable");
        if (tabButtons.Count <= 0) { return; }

        if (resetOnEnable)
            ResetTab();
    }

    public void OnTabClicked(UITabButton button)
    {
        foreach (UITabButton b in tabButtons)
        {
            if(b == button)            
                b.SetActive(true);            
            else 
                b.SetActive(false);
        }
    }

    public void ResetTab()
    {
        if (initialTabIndex >= tabButtons.Count)
            initialTabIndex = 0;

        OnTabClicked(tabButtons[initialTabIndex]);
        tabButtons[initialTabIndex].InvokeRegardless();
    }
}
