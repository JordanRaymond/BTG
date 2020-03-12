using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class HitEvent : UnityEvent<HitPrecision> { }

[System.Serializable]
public class UserAction
{
    public HitEvent hitEvent;

    [SerializeField] private float actionTimeInBeat;
    [SerializeField] private InputButton expectedInput;
    [SerializeField] private bool isLongPress = false; // Todo : find better name
    [Tooltip(
        "Represent the length in beat that the input are accepted. Ex : if the beat duration is 0.8 ,the timespanLength is 0.5, " +
        "and the action time is 3, the zone will be 2.6 to 3 and 3 to 3.4."
        )]
    [SerializeField] private float timespanLengthInBeat = 0.5f;
    [SerializeField] private float mediumPrecisionPercentage = 45;
    [SerializeField] private float goodPrecisionPercentage = 60;
    [SerializeField] private float perfectPrecisionPercentage = 75;

    public UserAction(float p_actionTimeInBeat, InputButton p_expectedInput) {
        actionTimeInBeat = p_actionTimeInBeat;
        expectedInput = p_expectedInput;
    }

#region Properties
    public InputButton ExpectedInput {
        get {
            return expectedInput;
        }
    }

    public float ActionTimeInBeat {
        get {
            return actionTimeInBeat;
        }
    }

    public float TimespanLengthInBeat {
        get {
            return timespanLengthInBeat;
        }
    }

    public float MediumPrecisionPercentage {
        get {
            return mediumPrecisionPercentage;
        }
    }

    public float GoodPrecisionPercentage {
        get {
            return goodPrecisionPercentage;
        }
    }

    public float PerfectPrecisionPercentage {
        get {
            return perfectPrecisionPercentage;
        }
    }
#endregion

    public bool IsActive(float songTimeInBeat, float beatDuration) {

        return (songTimeInBeat >= actionTimeInBeat - beatDuration)
                &&
                (songTimeInBeat <= actionTimeInBeat + beatDuration);
    }

    public void InputMissed() {
        //  HitPrecision hitPrecision = HitPrecision.Missed;

        // hitEvent.Invoke(hitPrecision);
    }

    public HitPrecision OnInput(InputButton userInput, float timeInBeats, float beatDuration) {
        HitPrecision hitPrecision;

        if (expectedInput != userInput) {
            hitPrecision = HitPrecision.Missed;
        }
        else {
            hitPrecision = GetInputPrecision(timeInBeats, beatDuration);
        }

        // hitEvent.Invoke(hitPrecision);

        return hitPrecision;
    }

    private HitPrecision GetInputPrecision(float inputTimeInBeats, float beatDuration) {

        float inputPrecision = GetInputPrecisionPercentage(inputTimeInBeats, beatDuration);

        if (inputPrecision > 100 || inputPrecision <= 0) {
            return HitPrecision.Missed;
        }
        if (inputPrecision > perfectPrecisionPercentage) {
            return HitPrecision.Perfect;
        }
        if (inputPrecision > goodPrecisionPercentage) {
            return HitPrecision.Good;
        }
        if (inputPrecision > mediumPrecisionPercentage) {
            return HitPrecision.Medium;
        }


        return HitPrecision.Bad;
    }

    public float GetInputPrecisionPercentage(float inputTimeInBeats, float beatDuration) {
        float thresholdLength = beatDuration * timespanLengthInBeat;

        float maxTime = actionTimeInBeat + thresholdLength;
        float minTime = actionTimeInBeat - thresholdLength;

        float elapsedTime = 0;
        if (inputTimeInBeats <= actionTimeInBeat) {
            elapsedTime = inputTimeInBeats - minTime;
        }
        else {
            elapsedTime = maxTime - inputTimeInBeats;
        }

        float inputPrecision = (elapsedTime / thresholdLength) * 100;

        return inputPrecision;
    }
}
