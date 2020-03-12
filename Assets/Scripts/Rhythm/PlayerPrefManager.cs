using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPrefManager : MonoBehaviour {
    const string MASTER_VOLUME_KEY = "master_volume";


    public static float MasterVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
        }

        set
        {
            if (value >= 0f && value <= 1f)
            {
                PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, value);
            }
            else
            {
                Debug.LogError("Master volume out of range");
            }
        }
    }
}