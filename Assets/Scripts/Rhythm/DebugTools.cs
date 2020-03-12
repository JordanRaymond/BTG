using System.Linq;
using UnityEngine;

public class DebugTools : MonoBehaviour
{

    private MasterTimeLine _masterTimeLine;
    private UserActionTimeLine _userActionTimeLine;

    float _sliderXPosition = 75;
    float _sliderWidth = Screen.width;

    float inputRectWidth = 1;
    float inputRectHeight = 5;
    Color playerInputRectColor = Color.green;

    [Header("Debug variables")]
    public float sliderLeftPadding = 100;
    public float timelineWindowLenghtSlider = 10;

    // TODO
    void Start()
    {
        if (!_masterTimeLine)
        {
            _masterTimeLine = FindObjectOfType<MasterTimeLine>();
        }
        if (!_userActionTimeLine)
        {
            _userActionTimeLine = FindObjectOfType<UserActionTimeLine>();
        }
    }

    void OnGUI()
    {
        _sliderWidth = Screen.width - sliderLeftPadding;

        // Timeline precision slider
        GUI.Label(new Rect(120, Screen.height - 100, 1000, 30), "Timeline precision : " + (int)timelineWindowLenghtSlider);
        timelineWindowLenghtSlider = GUI.HorizontalSlider(new Rect(10, Screen.height - 95, 100, 30), timelineWindowLenghtSlider, 1.0F, 100.0F);

        GUIDrawHeader();

        GUIDrawUserActions();

        GUIDrawUserInputs();

        GUIDrawTimelines();

    }

    private void GUIDrawUserActions()
    {
        for (int i = 0; i < _userActionTimeLine.UserActions.Count; i++)
        {
            float windowLength = _masterTimeLine.BeatDuration * timelineWindowLenghtSlider;
            float timespanLengthInBeat = _userActionTimeLine.UserActions[i].TimespanLengthInBeat * _masterTimeLine.BeatDuration;

            float badTimeZoneXMinPosition = CalculateXPositionOnTimeline(windowLength, _userActionTimeLine.UserActions[i].ActionTimeInBeat - timespanLengthInBeat);
            float badTimeZoneXMaxPosition = CalculateXPositionOnTimeline(windowLength, _userActionTimeLine.UserActions[i].ActionTimeInBeat + timespanLengthInBeat);

            if (badTimeZoneXMinPosition < _sliderWidth & badTimeZoneXMaxPosition > 0)
            {

                float actionYPosition = _userActionTimeLine.UserActions[i].ExpectedInput == InputButton.Button1 ? Screen.height - 41 : Screen.height - 22;

                // -Bad TimespanZone-

                badTimeZoneXMinPosition = badTimeZoneXMinPosition > 0 ? badTimeZoneXMinPosition : 0;
                badTimeZoneXMaxPosition = badTimeZoneXMaxPosition < _sliderWidth ? badTimeZoneXMaxPosition : _sliderWidth;

                float badTimeWidth = (badTimeZoneXMaxPosition - badTimeZoneXMinPosition) > 0 ? badTimeZoneXMaxPosition - badTimeZoneXMinPosition : 0;
                // !Bad TimespanZone! 

                // -Medium TimespanZone-
                float precisionPercentage = (100 - _userActionTimeLine.UserActions[i].MediumPrecisionPercentage) / 100;
                float mediumTimeZoneXMinPosition = CalculateXPositionOnTimeline(windowLength, _userActionTimeLine.UserActions[i].ActionTimeInBeat - timespanLengthInBeat * precisionPercentage);
                float mediumTimeZoneXMaxPosition = CalculateXPositionOnTimeline(windowLength, _userActionTimeLine.UserActions[i].ActionTimeInBeat + timespanLengthInBeat * precisionPercentage);

                mediumTimeZoneXMinPosition = mediumTimeZoneXMinPosition > 0 ? mediumTimeZoneXMinPosition : 0;
                mediumTimeZoneXMaxPosition = mediumTimeZoneXMaxPosition < _sliderWidth ? mediumTimeZoneXMaxPosition : _sliderWidth;

                float mediumTimeWidth = (mediumTimeZoneXMaxPosition - mediumTimeZoneXMinPosition) > 0 ? mediumTimeZoneXMaxPosition - mediumTimeZoneXMinPosition : 0;
                // !Medium TimespanZone! 

                // -Good TimespanZone-
                precisionPercentage = (100 - _userActionTimeLine.UserActions[i].GoodPrecisionPercentage) / 100;
                float goodTimeZoneXMinPosition = CalculateXPositionOnTimeline(windowLength, _userActionTimeLine.UserActions[i].ActionTimeInBeat - timespanLengthInBeat * precisionPercentage);
                float goodTimeZoneXMaxPosition = CalculateXPositionOnTimeline(windowLength, _userActionTimeLine.UserActions[i].ActionTimeInBeat + timespanLengthInBeat * precisionPercentage);

                goodTimeZoneXMinPosition = goodTimeZoneXMinPosition > 0 ? goodTimeZoneXMinPosition : 0;
                goodTimeZoneXMaxPosition = goodTimeZoneXMaxPosition < _sliderWidth ? goodTimeZoneXMaxPosition : _sliderWidth;

                float goodTimeWidth = (goodTimeZoneXMaxPosition - goodTimeZoneXMinPosition) > 0 ? goodTimeZoneXMaxPosition - goodTimeZoneXMinPosition : 0;
                // !Good TimespanZone! 

                // -Perfect TimespanZone-
                precisionPercentage = (100 - _userActionTimeLine.UserActions[i].PerfectPrecisionPercentage) / 100;
                float perfectTimeZoneXMinPosition = CalculateXPositionOnTimeline(windowLength, _userActionTimeLine.UserActions[i].ActionTimeInBeat - timespanLengthInBeat * precisionPercentage);
                float perfectTimeZoneXMaxPosition = CalculateXPositionOnTimeline(windowLength, _userActionTimeLine.UserActions[i].ActionTimeInBeat + timespanLengthInBeat * precisionPercentage);

                perfectTimeZoneXMinPosition = perfectTimeZoneXMinPosition > 0 ? perfectTimeZoneXMinPosition : 0;
                perfectTimeZoneXMaxPosition = perfectTimeZoneXMaxPosition < _sliderWidth ? perfectTimeZoneXMaxPosition : _sliderWidth;

                float perfectTimeZoneXPositionTimeWidth = (perfectTimeZoneXMaxPosition - perfectTimeZoneXMinPosition) > 0 ? perfectTimeZoneXMaxPosition - perfectTimeZoneXMinPosition : 0;
                // !Perfect TimespanZone! 

                // !Bad TimespanZone! 
                GUIDrawRect(new Rect(_sliderXPosition + badTimeZoneXMinPosition, actionYPosition, badTimeWidth, inputRectHeight), Color.red);
                // !Medium TimespanZone! 
                GUIDrawRect(new Rect(_sliderXPosition + mediumTimeZoneXMinPosition, actionYPosition, mediumTimeWidth, inputRectHeight), Color.yellow);
                // !Good TimespanZone! 
                GUIDrawRect(new Rect(_sliderXPosition + goodTimeZoneXMinPosition, actionYPosition, goodTimeWidth, inputRectHeight), Color.green);
                // !Perfect TimespanZone! 
                GUIDrawRect(new Rect(_sliderXPosition + perfectTimeZoneXMinPosition, actionYPosition, perfectTimeZoneXPositionTimeWidth, inputRectHeight), Color.blue);
            }
        }
    }

    private void GUIDrawUserInputs()
    {
        for (int i = 0; i < _userActionTimeLine.UserInputs.Count; i++)
        {
            float windowLength = _masterTimeLine.BeatDuration * timelineWindowLenghtSlider;

            if (_userActionTimeLine.UserInputs[i].inputTimeInBeat < _masterTimeLine.SongPosInBeats + windowLength & _userActionTimeLine.UserInputs[i].inputTimeInBeat > _masterTimeLine.SongPosInBeats - windowLength)
            {
                float actionXMarkPosition = CalculateXPositionOnTimeline(windowLength, _userActionTimeLine.UserInputs[i].inputTimeInBeat);
                float actionYPosition = _userActionTimeLine.UserInputs[i].userInput == InputButton.Button1 ? Screen.height - 45 : Screen.height - 28;

                GUIDrawRect(new Rect(_sliderXPosition + actionXMarkPosition, actionYPosition, inputRectWidth, inputRectHeight + 10), playerInputRectColor);
            }
        }
    }

    private float CalculateXPositionOnTimeline(float windowLengthInBeat, float timePositionInBeat)
    {
        float targetPosition = _sliderWidth / 2;
        float distanceFromTargetPosition = (_masterTimeLine.SongPosInBeats + windowLengthInBeat) - timePositionInBeat;

        return _sliderWidth - (targetPosition * (distanceFromTargetPosition / windowLengthInBeat));
    }

    Rect button1Rect;
    Rect button2Rect;
    private void GUIDrawTimelines()
    {
        button1Rect = new Rect(_sliderXPosition, Screen.height - 44, _sliderWidth, 0);
        button2Rect = new Rect(_sliderXPosition, Screen.height - 26, _sliderWidth, 0);

        GUI.Label(new Rect(10, Screen.height - 50, 200, 200), "Button 1");
        GUI.HorizontalSlider(button1Rect, _sliderWidth / 2, 0.0F, _sliderWidth);

        GUI.Label(new Rect(10, Screen.height - 32, 200, 200), "Button 2");
        GUI.HorizontalSlider(button2Rect, _sliderWidth / 2, 0.0F, _sliderWidth);
    }

    private void GUIDrawHeader()
    {
        UserInput userAction = _userActionTimeLine.UserInputs.LastOrDefault();

        GUI.Label(new Rect(10, Screen.height - 84, 200, 200), "Song Position : " + _masterTimeLine.SongPosition);
        GUI.Label(new Rect(10, Screen.height - 71, 200, 200), "Song Position in beat : " + _masterTimeLine.SongPosInBeats);
        GUI.Label(new Rect(210, Screen.height - 84, 200, 200), "Last Input : " + userAction.userInput);
        GUI.Label(new Rect(210, Screen.height - 71, 200, 400), "Last Input precision: " + (int)userAction.inputPrecisionInPercentage + "%" + " " + userAction.inputPrecision);
    }

    private static Texture2D staticRectTexture;
    private static GUIStyle staticRectStyle;
    public static void GUIDrawRect(Rect position, Color color)
    {
        if (staticRectTexture == null)
        {
            staticRectTexture = new Texture2D(1, 1);
        }

        if (staticRectStyle == null)
        {
            staticRectStyle = new GUIStyle();
        }

        staticRectTexture.SetPixel(0, 0, color);
        staticRectTexture.Apply();

        staticRectStyle.normal.background = staticRectTexture;

        GUI.Box(position, GUIContent.none, staticRectStyle);
    }
}
