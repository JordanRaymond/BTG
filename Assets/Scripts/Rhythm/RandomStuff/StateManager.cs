using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {

    enum State { Walking, Sprinting, Crouching }

    private State currentState;

	void Start () {
        currentState = State.Walking;
	}
	
	void Update () {
		
	}

    void FixedUpdate() {
        switch (currentState) {
            case State.Walking:
                Walk();
                break;
            case State.Sprinting:
                Sprint();
                break;
            case State.Crouching:
                Crouch();
                break;
            default:
                break;
        }
    }

    private void Crouch() {
        throw new NotImplementedException();
    }

    private void Sprint() {
        throw new NotImplementedException();
    }

    private void Walk() {
        throw new NotImplementedException();
    }
}
