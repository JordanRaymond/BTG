using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class UserActionTimeLine : MonoBehaviour
{
    private MasterTimeLine masterTimeLine;

    [SerializeField] private List<UserAction> userActions;
    [SerializeField] private List<UserAction> ActiveUserActions;
    private List<UserInput> userInputs = new List<UserInput>();

    private int nextUserActionIndex = 0;

    #region Properties
    public List<UserAction> UserActions
    {
        get
        {
            return userActions;
        }
    }

    public List<UserInput> UserInputs
    {
        get
        {
            return userInputs;
        }
    }
    #endregion

    // TODO
    void Start()
    {
        if (!masterTimeLine)
        {
            masterTimeLine = FindObjectOfType<MasterTimeLine>();
        }
    }

    void Update()
    {
        CheckNextUserActionReady();
        UpdateActiveUserActionsList();

        // TODO : Input manager event of type UserInput
        if (Input.GetButtonDown("Button 1") || Input.GetButtonDown("Button 2"))
        {
            InputButton userInput;

            if (Input.GetButton("Button 1") & Input.GetButtonDown("Button 2"))
            {
                userInput = InputButton.Button1And2;
            }
            else if (Input.GetButton("Button 1"))
            {
                userInput = InputButton.Button1;
            }
            else
            {
                userInput = InputButton.Button2;
            }

            Debug.Log(userInput);
            OnIput(userInput);
        }
    }

    private void UpdateActiveUserActionsList()
    {
        for (int i = 0; i < ActiveUserActions.Count; i++)
        {
            if (!ActiveUserActions[i].IsActive(masterTimeLine.SongPosInBeats, masterTimeLine.BeatDuration))
            {
                ActiveUserActions[i].InputMissed();

                ActiveUserActions.Remove(ActiveUserActions[i]);
            }
        }
    }

    private void CheckNextUserActionReady()
    {
        if (nextUserActionIndex < UserActions.Count)
        {
            if (UserActions[nextUserActionIndex].IsActive(masterTimeLine.SongPosInBeats, masterTimeLine.BeatDuration))
            {
                ActiveUserActions.Add(UserActions[nextUserActionIndex]);

                nextUserActionIndex++;
            }
        }
    }

    private void OnIput(InputButton buttonPress)
    {
        UserAction action = ActiveUserActions.FirstOrDefault();
        UserInput userInput = new UserInput(masterTimeLine.SongPosInBeats, buttonPress);

        if (action != null)
        {
            HitPrecision hitPrecision = action.OnInput(buttonPress, masterTimeLine.SongPosInBeats, masterTimeLine.BeatDuration);

            userInput.inputPrecision = hitPrecision;
            userInput.inputPrecisionInPercentage = action.GetInputPrecisionPercentage(masterTimeLine.SongPosInBeats, masterTimeLine.BeatDuration);

            ActiveUserActions.Remove(action);
        }

        userInputs.Add(userInput);
    }
}