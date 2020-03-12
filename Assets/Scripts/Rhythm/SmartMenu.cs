using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SmartMenu : MonoBehaviour
{

    public List<GameObject> menusToDisable;
    public Animator cameraAnimator;
    public float timeAutoLoadNextLevel = 0;

    [Header("Settings options")]
    public Slider volumeSlider;

    private MusicManager musicManager;

    void Awake() {
        // tag = "Level Manager";
    }

    void Start() {
        if (timeAutoLoadNextLevel > 0) LoadNextLevel(timeAutoLoadNextLevel);
        else Debug.Log("Auto Load Disable");

        // TODO - dont use findGameObject ;K|
        // musicManager = GameObject.FindGameObjectWithTag("Music Manager").GetComponent<MusicManager>();

        // volumeSlider.value = PlayerPrefManager.MasterVolume;
    }

    // TODO - hmm, always update? maybe if option menu is active || when you go to the setting, make audio actvie and turn flag on
    //void FixedUpdate() {
    //    musicManager.ChangeVolume(volumeSlider.value);
    //}

    public void LoadLevel(string name) {
        SceneManager.LoadScene(name);
    }

    public static void LoadNextLevel() {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex) {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public static void QuitRequest() {
        Debug.Log("Quit requested");
        Application.Quit();
    }

    public void DisableOtherMenus() {
        foreach (GameObject menu in menusToDisable) {
            menu.SetActive(false);
        }
    }

    public void DisableMenu(GameObject obj) {
        DisableOtherMenus();

        obj.SetActive(!obj.activeSelf);
    }

    public void DisableObj(GameObject obj) {
        obj.SetActive(!obj.activeSelf);
    }

    public void LoadNextLevel(float time) {
        StartCoroutine(WaitLoadNextLevel(time));
    }

    public void LoadLevel(string name, float time) {
        StartCoroutine(WaitLoadScene(name, time));
    }

    public void TriggerCamAnimation(string triggerName) {
        cameraAnimator.SetTrigger(triggerName);
    }

    // ==================== OPTIONS MENU ====================
    public void SaveAndExit() {
        PlayerPrefManager.MasterVolume = volumeSlider.value;
    }

    public void SetDefault() {
        volumeSlider.value = 0.8f;
    }
    // !==================== OPTIONS MENU ====================!

    private IEnumerator WaitLoadNextLevel(float time) {
        yield return new WaitForSeconds(time);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCountInBuildSettings > nextSceneIndex) {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    private IEnumerator WaitLoadScene(string name, float time) {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(name);
    }
}
