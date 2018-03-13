using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInspectable {

    GameObject Target { get; }
    bool UIFollow { get; }
    void OnStartInspect();
    void OnStopInspect();
    List<Attribute> GetAttributes();
}
