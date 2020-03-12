using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour {

    public List<Combo> combosList;

    private Dictionary<List<ComboInput>, Combo> comboDictionary; // Todo, properties or something

    void Start () {
        comboDictionary = new Dictionary<List<ComboInput>, Combo>(new ComboDictionaryComparer());

        for (int i = 0; i < combosList.Count; i++) {
            comboDictionary.Add(combosList[i].inputNamesList, combosList[i]);
        }
    }
	
    public Combo GetCombo(List<ComboInput> combosList) {
        Combo combo;
        comboDictionary.TryGetValue(combosList, out combo);

        return combo;
    }
}
