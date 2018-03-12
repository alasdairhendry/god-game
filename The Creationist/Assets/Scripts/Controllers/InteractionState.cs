using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionState : MonoBehaviour {

    public static InteractionState singleton;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);
    }

    public enum State { Free, Paused, PlacingEntity }
    [SerializeField] private State currentState = State.Free;
    public State CurrentState { get { return currentState; } }

    private State beforePause = State.Free; // Remember the state before pause so you can return to it!

    public void Pause(bool state)
    {
        if (state)
        {
            beforePause = currentState;
            currentState = State.Paused;
        }
        else
        {
            currentState = beforePause;
        }
    }

    public void PauseUnpause()
    {
        if (currentState == State.Paused) Pause(false);
        else Pause(true);
    }

    public void SetPlacingEntity(bool state)
    {
        if (state)
            currentState = State.PlacingEntity;
        else currentState = State.Free;
    }
}
