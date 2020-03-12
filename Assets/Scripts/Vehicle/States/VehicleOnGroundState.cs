using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleOnGroundState : IVehicleState
{

    public void OnEnteredState(VehicleParams vehicleParams)
    {

    }

    public void UpdateState(VehicleParams vehicleParams)
    {

    }

    public void FixedUpdateState(VehicleParams vehicleParams)
    {
        Move(vehicleParams);
    }

    public void OnExitState(VehicleParams vehicleParams)
    {

    }


    private void Move(VehicleParams vehicleParams)
    {
        vehicleParams.vehicle.Move(1);
    }

    public string GetName()
    {
        return "VehicleOnGroundState";
    }
}
