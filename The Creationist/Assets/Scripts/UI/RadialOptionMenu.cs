using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialOptionMenu {

    private Action onClick;
    public Action OnClick { get { return onClick; } }

    private Sprite icon;
    public Sprite Icon { get { return icon; } }

    private string toolTip = "";
    private bool displayTooltip = false;

    private bool autoDeselect = false;
    public bool AutoDeselect { get { return autoDeselect; } }

    public RadialOptionMenu(Action onClick, Sprite icon)
    {
        this.onClick = onClick;
        this.icon = icon;
    }

    public RadialOptionMenu(Action onClick, Sprite icon, bool autoDeselect)
    {
        this.onClick = onClick;
        this.icon = icon;
        this.autoDeselect = autoDeselect;
    }
}
