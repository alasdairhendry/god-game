using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Logger {

    //public static void Log()
    //{

    //}

    public static void LogSelected(object message, GameObject from, Object Context)
    {
        return;
        if (Selection.activeGameObject == from)
            Debug.Log(Context.GetType() + ": " + message, Context);
    }

}
