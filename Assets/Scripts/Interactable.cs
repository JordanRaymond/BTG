using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string txtToShow = "";
    // Check if player looking at component
    // Show icone to focus
    // If the input handler said buton press call the onInteract on a IIteractble component on the gameobject
    void Start()
    {
        if (gameObject.layer != LayerMask.NameToLayer("Interactable"))
        {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }
    }

    void Update()
    {

    }
}
