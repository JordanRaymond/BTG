using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComboInput { LeftDrum, RightDrum, TopDrum, DownDrum }

[RequireComponent(typeof(AudioSource))]
public class DrumsInput : MonoBehaviour
{
    public AudioClip failSound;

    [Header("Drum sounds")]
    public AudioClip leftDrumSound;
    public AudioClip rightDrumSound;
    public AudioClip topDrumSound;
    public AudioClip downDrumSound;

    private RhythmManager rhythmManager;
    private ComboManager comboManager;

    private AudioSource audioSrc;
    private double lastInputTime = 0.0f;

    private List<ComboInput> inputList;
    private Dictionary<List<ComboInput>, string> comboDictionary;

    private bool keyWasDown = false;

    void Start()
    {
        inputList = new List<ComboInput>();

        comboDictionary = new Dictionary<List<ComboInput>, string>(new ComboDictionaryComparer());

        rhythmManager = GetComponentInParent<RhythmManager>();
        comboManager = FindObjectOfType<ComboManager>();

        audioSrc = GetComponent<AudioSource>();

        lastInputTime = AudioSettings.dspTime;
    }

    void Update()
    {
        ReadInput();
        if (keyWasDown)
        {
            // If the time elapsed since the last input is more than
            if (((AudioSettings.dspTime - lastInputTime) / rhythmManager.TimeBtwTick) * 100 > 110)
            {
                keyWasDown = false;
                inputList.Clear();
            }
        }
    }

    void ReadInput()
    {
        if (Input.GetButtonDown("XBoxX"))
        {
            audioSrc.PlayOneShot(leftDrumSound);
            OnKeyDown(ComboInput.LeftDrum);
        }
        if (Input.GetButtonDown("XBoxB"))
        {
            audioSrc.PlayOneShot(rightDrumSound);
            OnKeyDown(ComboInput.RightDrum);
        }
        if (Input.GetButtonDown("XBoxY"))
        {
            audioSrc.PlayOneShot(topDrumSound);
            OnKeyDown(ComboInput.TopDrum);
        }
        if (Input.GetButtonDown("XBoxA"))
        {
            audioSrc.PlayOneShot(downDrumSound);
            OnKeyDown(ComboInput.DownDrum);
        }
    }

    void OnKeyDown(ComboInput keyName)
    {
        double inputTime = AudioSettings.dspTime;
        double elapsedTimeSinceLastInput = inputTime - lastInputTime;
        keyWasDown = true;

        Timing timing = rhythmManager.GetInputTimming(inputTime);

        // If the timing is bad or the time bwt the input is too close
        if (timing == Timing.Bad || elapsedTimeSinceLastInput <= (rhythmManager.TimeBtwTick / 2))
        {
            inputList.Clear();
            audioSrc.PlayOneShot(failSound);
        }
        else
        {
            inputList.Add(keyName);
            CheckIsCombo();

            Debug.Log("============ COMBO LIST ==============");
            string comboString = "";
            foreach (ComboInput input in inputList)
            {
                comboString += input + " ";
            }
            Debug.Log(comboString);
            Debug.Log("=======================================");

        }
        lastInputTime = AudioSettings.dspTime;

        Debug.Log(rhythmManager.GetInputTimming(inputTime));
    }

    private void CheckIsCombo()
    {
        // TODO : Check in the list of array of combo input if (in order) the inpurt match
        Combo combo = comboManager.GetCombo(inputList);
        if (combo != null)
        {
            Debug.Log("COMBO!!!!!");
            combo.Initialize(GameObject.FindGameObjectWithTag("Player"));
            combo.TriggerComboAction();

            inputList.Clear();
        }
    }


    /// <summary>
    /// Only for debug purpose
    /// </summary>
    void DicTest()
    {
        List<ComboInput> a1 = new List<ComboInput>(new ComboInput[] { ComboInput.LeftDrum, ComboInput.RightDrum });
        List<ComboInput> a2 = new List<ComboInput>(new ComboInput[] { ComboInput.LeftDrum, ComboInput.DownDrum });
        comboDictionary.Add(a1, "Combo1");
        comboDictionary.Add(a2, "Combo2");

        foreach (KeyValuePair<List<ComboInput>, string> item in comboDictionary)
        {
            for (int i = 0; i < item.Key.Count; i++)
            {
                Debug.Log(item.Key[i]);
            }
            Debug.Log(item.Key + " = " + item.Value);
        }
        string value;
        comboDictionary.TryGetValue(a1, out value);

        Debug.Log("Dic count : " + comboDictionary.Count);
        Debug.Log("Dic a1 value : " + value);
    }
}
