using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class VehicleController : MonoBehaviour
{
    public VehicleParams defaultVehicleParams;
    [HideInInspector] public InputHandler inputHandler;

    #region State vars
    public VehicleOnGroundState defaultState = new VehicleOnGroundState();
    private IVehicleState currentState;
    #endregion 

    void Start()
    {
        inputHandler = GameManager.InputHandler;
        InitParams();
        SetState(defaultState);
    }

    private void InitParams()
    {
        defaultVehicleParams.vehicle = this;
    }

    void Update()
    {
        currentState.UpdateState(defaultVehicleParams);
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(defaultVehicleParams);
    }

    public void Move(float speed)
    {
        Vector3 dir = transform.forward;

        // Then add the direction * the speed to the current position:
        transform.position += dir * speed * Time.deltaTime;
    }

    public void SetState(IVehicleState state)
    {
        if (state != null)
        {
            // Debug.Log("State set:" + state.GetName());
            if (currentState != null)
            {
                currentState.OnExitState(defaultVehicleParams);
            }
            currentState = state;
            currentState.OnEnteredState(defaultVehicleParams);
        }
        else
        {
            Debug.LogError("VehicleController: Can't enter null state");
        }
    }
}
