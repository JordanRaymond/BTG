using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BeatEvents : UnityEvent<double> { }

public class MetronomeBpm : MonoBehaviour
{

    #region Debug variables

    #endregion

    public BeatEvent beatEvent; // Event call at each beat
    public AudioClip firstClip;
    public AudioClip secondClip;
    public int tickSoundIndex = 1;


    [SerializeField]
    private int numberOfSteps = 4;
    [Range(1, 300), SerializeField]
    private double bpm = 120.0F;
    private double nextTick = 0.0F;     // Next tick time in dspTime
    private double lastTick = 0.0F;     // Last tick time in dspTime
    private double timeBtwTick = 0.0F;  // Time between 2 tick

    private bool useDynamicTickTime = false; // To calculate timeBtwTick - if true -> lastTickTime - nextTickTime else -> 60.0 / bpm
    private bool ticked = false;
    private double dspTime = 0;
    private AudioSource audioSource;

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
            if (value > 0 && value < 300)
            {
                bpm = value;
            }
            else
            {
                Debug.LogError("Bpm out of bounds!");
            }
        }
    }

    // Number of steps of the metronome 
    public int NumberOfSteps
    {
        get
        {
            return numberOfSteps;
        }
        set
        {
            if (value > 0)
            {
                numberOfSteps = value;
            }
            else
                numberOfSteps = 2;
        }
    }

    public bool UseDynamicTickTime
    {
        get
        {
            return useDynamicTickTime;
        }
        set
        {
            if (!value)
            {
                timeBtwTick = 60 / bpm; // set timeBtwTick back to correct value if don't use dynamic time
            }

            useDynamicTickTime = value;
        }
    }
    #endregion

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        nextTick = AudioSettings.dspTime + (60.0 / bpm);
        timeBtwTick = 60 / bpm;
    }

    private void Update()
    {
        dspTime = AudioSettings.dspTime;
        UpdateNextTick();

        if (!ticked && nextTick >= dspTime)
        {
            ticked = true;

            // BroadcastMessage("OnTick"); <- Broadcast is slow and hard to debug/track
            OnTick();

            beatEvent.Invoke(nextTick);
        }

        if (useDynamicTickTime)
        {
            timeBtwTick = nextTick - lastTick;
        }
    }

    void UpdateNextTick()
    {
        double timePerTick = 60 / bpm;

        while (dspTime >= nextTick)
        {
            ticked = false;
            nextTick += timePerTick;
        }
    }

    void OnTick()
    {
        PlayMetronome();

        lastTick = AudioSettings.dspTime;
    }

    private void PlayMetronome()
    {
        audioSource.PlayOneShot(tickSoundIndex < numberOfSteps ? firstClip : secondClip);
        tickSoundIndex += tickSoundIndex < numberOfSteps ? 1 : -numberOfSteps + 1;
    }
}
