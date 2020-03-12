using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// [System.Serializable]
// public class BeatEvent : UnityEvent<double> { }

public enum Timing { Bad, Mid, Good, Perfect }

public class RhythmManager : MonoBehaviour
{

    #region Debug variables
    //public Text lastTickText;
    //public Text nextTickText;
    //public Text precisionText;
    //public Text timeBtwTickText;
    //public Text lastInpuTimeText;
    //public Text elapsedTimeSinceInputText;

    //public GameObject image;
    #endregion

    public BeatEvent beatEvent; // Event launch at each beat

    [Range(35, 300), SerializeField]
    private double bpm = 120.0F;
    public AudioClip firstClip;
    public AudioClip secondClip;

    private double nextTick = 0.0F; // The next tick in dspTime
    private double lastTick = 0.0F; // The last tick in dspTime
    private double timeBtwTick = 0.0F;
    private bool ticked = false;

    #region Properties
    public double NextTick
    {
        get
        {
            return nextTick;
        }
    }

    public double LastTick
    {
        get
        {
            return lastTick;
        }
    }

    public double TimeBtwTick
    {
        get
        {
            return timeBtwTick;
        }
    }

    public double Bpm
    {
        get
        {
            return bpm;
        }
        set
        {
            bpm = value;
        }
    }
    #endregion

    void Start()
    {
        double startTick = AudioSettings.dspTime;
        nextTick = startTick + (60.0 / bpm);
    }

    private void Update()
    {
        UpdateNextTick();
    }

    void LateUpdate()
    {
        if (!ticked && nextTick >= AudioSettings.dspTime)
        {
            ticked = true;
            BroadcastMessage("OnTick");

            beatEvent.Invoke(nextTick); // Todo change to int 
        }

        timeBtwTick = nextTick - lastTick;
        // UpdateText();
    }

    void UpdateNextTick()
    {
        double timePerTick = 60 / bpm;
        double dspTime = AudioSettings.dspTime;

        while (dspTime >= nextTick)
        {
            ticked = false;
            nextTick += timePerTick;
        }
    }

    //void UpdateText() {
    //    lastTickText.text = lastTick.ToString();
    //    nextTickText.text = nextTick.ToString();

    //    timeBtwTickText.text = timeBtwTick.ToString();

    //    if (Input.GetButtonDown("Jump")) {
    //        SetInputPrecisionTxt();
    //    }
    //}

    //void SetInputPrecisionTxt() {
    //    double t = AudioSettings.dspTime;
    //    lastInpuTimeText.text = t.ToString();

    //    double elapsedTimeSinceInput = t - lastTick; // Elapsed time since lastTick to inputTime
    //    double r = elapsedTimeSinceInput / timeBtwTick;

    //    precisionText.text = (r * 100).ToString();
    //    elapsedTimeSinceInputText.text = elapsedTimeSinceInput.ToString();
    //}

    private int tickSoundIndex = 0;
    void OnTick()
    {
        GetComponent<AudioSource>().PlayOneShot(tickSoundIndex <= 2 ? firstClip : secondClip);
        tickSoundIndex += tickSoundIndex <= 2 ? 1 : -3;

        // image.SetActive(!image.activeSelf);
        lastTick = AudioSettings.dspTime;
    }

    public Timing GetInputTimming(double inputTime)
    {
        double elapsedTimeSinceInput = inputTime - lastTick; // Elapsed time since lastTick to inputTime
        double ratio = (elapsedTimeSinceInput / timeBtwTick) * 100;
        // SetInputPrecisionTxt();

        if (ratio > 41 && ratio < 61)
            return Timing.Bad;

        if (ratio > 30 && ratio < 70)
            return Timing.Mid;

        if (ratio > 15 && ratio < 85)
            return Timing.Good;
        else
            return Timing.Perfect;
    }
}
