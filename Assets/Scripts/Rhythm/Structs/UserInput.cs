public struct UserInput
{
    public float inputTimeInBeat;
    public InputButton userInput;
    public HitPrecision inputPrecision;
    public float inputPrecisionInPercentage;

    public UserInput(float p_inputTimeInBeat, InputButton p_inputButton, HitPrecision p_hitPrecision = HitPrecision.Bad, float p_inputPrecisionInPercentage = 0) {
        inputTimeInBeat = p_inputTimeInBeat;
        userInput = p_inputButton;
        inputPrecision = p_hitPrecision;
        inputPrecisionInPercentage = p_inputPrecisionInPercentage;
    }
}
