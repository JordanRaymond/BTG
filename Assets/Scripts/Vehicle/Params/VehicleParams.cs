using UnityEngine;

[CreateAssetMenu(fileName = "VehicleParams", menuName = "ScriptableObjects/VehicleParamsScriptableObj", order = 3)]
public class VehicleParams : Params<VehicleParams>
{
    public VehicleController vehicle;
    public override void UpdateValues(VehicleParams newValues)
    {
        Debug.LogError("Not implemented yet");
    }
}