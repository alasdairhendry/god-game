using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification {

    private Sprite icon;
    public Sprite Icon { get { return icon; } }

    private string header = "";
    public string Header { get { return header; } }

    private string message = "";
    public string Message { get { return message; } }

    private Action action;

    public Notification(Sprite icon, string header, string message)
    {
        this.icon = icon;
        this.header = header;
        this.message = message;
    }

    public Notification(Sprite icon, string header, string message, Action actionOnClick)
    {
        this.icon = icon;
        this.header = header;
        this.message = message;
        this.action = actionOnClick;
    }

    public Notification(string icon, string header, string message)
    {
        this.icon = Resources.Load<Sprite>(icon) as Sprite;
        this.header = header;
        this.message = message;
    }

    public Notification(string icon, string header, string message, Action actionOnClick)
    {
        this.icon = Resources.Load<Sprite>(icon) as Sprite;
        this.header = header;
        this.message = message;
        this.action = actionOnClick;
    }

    public Notification SetAction_RotateToGameObject(GameObject target)
    {
        action = () => {

            GameObject.FindObjectOfType<CameraOrbitMotion>().SetFollow(target);

        };
        return this;
    }

    public void InvokeAction()
    {
        if (action != null)
            action();
    }
}
