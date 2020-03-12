using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combos/MoveCombo")]
public class MoveCombo : Combo
{

    public Vector2 distance = Vector2.zero;
    // public float movementSpeed = 2f;

    private RhythmManager rhythmManager;
    private FPS.FpsController controller; // TODO
    private Transform playerTransform;

    public override void Initialize(GameObject obj)
    {
        playerTransform = obj.transform;

        controller = obj.GetComponent<FPS.FpsController>();
        rhythmManager = FindObjectOfType<RhythmManager>();
    }

    public override void TriggerComboAction()
    {
        double duration = rhythmManager.TimeBtwTick * 2;
        // controller.MoveInTime(distance, duration);
    }

}
