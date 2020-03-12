using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public abstract class Combo : ScriptableObject {
    public string comboName = "New Combo";
    public List<ComboInput> inputNamesList;

    public abstract void Initialize(GameObject obj);
    public abstract void TriggerComboAction();
}
