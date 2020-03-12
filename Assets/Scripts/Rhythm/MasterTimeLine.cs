using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BeatEvent : UnityEvent<double> { }

[RequireComponent(typeof(AudioSource))]
public class MasterTimeLine : MonoBehaviour
{
    public BeatEvent beatEvent; // Event call at each beat

    [Header("Song Settings")]
    public Song song;
    public float songDelay = 0;

    [Header("Metronome")]
    [SerializeField] private bool metronomeOn = false;
    [SerializeField] private int numberOfSteps = 4;
    public AudioClip firstClip;
    public AudioClip secondClip;
    private int tickSoundIndex = 1;

    [Header("----------")]
    [SerializeField, Range(1, 300)]
    private float bpm = 112;
    private double masterTime;
    private double masterPositionInBeat;

    private float songPosition; // the current position of the song (in seconds)
    private float songPosInBeats; // the current position of the song (in beats)
    private float beatDuration; // the duration of a beat

    private float songStartTime = 0.0F; // how much time (in seconds) has passed since the song started
    private double nextTick = 0.0F;
    private double lastTick = 0.0F;

    [Tooltip(
    "Change how the time between each beat is calculated. " +
    "If true -> beatDuration = lastTickTime - nextTickTime; if false " +
    "-> beatDuration = 60.0 / Bpm; ")]
    [SerializeField]
    private bool useDynamicTickTime = false;
    private bool ticked = false;

    private AudioSource audioSource;
    private double startTime;

    #region Properties
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
                beatDuration = 60 / bpm;
            }

            useDynamicTickTime = value;
        }
    }

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

    public float Bpm
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

    public float SongPosition
    {
        get
        {
            return songPosition;
        }
    }

    public float SongPosInBeats
    {
        get
        {
            return songPosInBeats;
        }
    }

    public double MasterPositionInBeat
    {
        get
        {
            return masterPositionInBeat;
        }
    }

    public float BeatDuration
    {
        get
        {
            return beatDuration;
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
    #endregion

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startTime = AudioSettings.dspTime;

        if (song)
        {
            bpm = song.Bpm;

            beatDuration = 60f / bpm;

            while (songStartTime < songDelay)
            {
                songStartTime += beatDuration;
            }

            audioSource.clip = song.AudioClip;
            audioSource.time = song.StartTime;
            audioSource.PlayScheduled(AudioSettings.dspTime + songStartTime);
        }
        else
        {
            songStartTime = (float)AudioSettings.dspTime;

            beatDuration = 60f / bpm;
        }

        nextTick = AudioSettings.dspTime + beatDuration;
    }

    void Update()
    {
        UpdateNextTick();

        if (!ticked && nextTick >= AudioSettings.dspTime)
        {
            ticked = true;

            OnTick();

            beatEvent.Invoke(nextTick);
        }

        if (useDynamicTickTime) // TODO, not good implementation, it was to be sure were sync with the audi thread
        {
            beatDuration = (float)(nextTick - lastTick);
        }

        masterTime = AudioSettings.dspTime - startTime;

        masterPositionInBeat = masterTime / beatDuration;
        if (song)
        {
            SongUpdate();
        }
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

    void OnTick()
    {
        if (metronomeOn)
        {
            PlayMetronome();
        }

        lastTick = AudioSettings.dspTime;
    }

    void SongUpdate()
    {
        if (audioSource.isPlaying)
        {
            // calculate the position in seconds
            songPosition = audioSource.time - song.StartTime;

            // calculate the position in beats
            songPosInBeats = songPosition / beatDuration;
        }

    }

    private bool SongCanPlay()
    {
        return masterTime >= songStartTime;
    }

    private void OnValidate()
    {
        if (!useDynamicTickTime)
        {
            beatDuration = 60 / bpm;
        }
    }

    private void PlayMetronome()
    {
        audioSource.PlayOneShot(tickSoundIndex < numberOfSteps ? firstClip : secondClip);
        tickSoundIndex += tickSoundIndex < numberOfSteps ? 1 : -numberOfSteps + 1;
    }
}
