public interface ITimeAction
{
    bool CanStart(float timeInBeat);
    void UpdateAction(float timeInBeat);
}
