using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioClip[] levelMusicChange;

    private GameObject instance;
    private AudioSource audioSource;
    private int activeSceneIndex;

    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneLoaded;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (instance != null) Destroy(gameObject);

        instance = gameObject;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnSceneLoaded(Scene lastScene, Scene newScene)
    {
        int index = newScene.buildIndex;
        if (index > levelMusicChange.Length - 1) return;

        AudioClip audioClip = levelMusicChange[index];

        if (!audioClip) return;

        audioSource.clip = audioClip;
        audioSource.Play();

        audioSource.volume = PlayerPrefManager.MasterVolume;
    }

    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
