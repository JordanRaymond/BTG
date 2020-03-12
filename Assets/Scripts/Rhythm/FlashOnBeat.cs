using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashOnBeat : MonoBehaviour
{
    private Image panelImage;
    private Color imageColor;

    private float coStartedTime = 0;
    private float fadingDuration = 0;
    private float coElapsedTime = 0;
    // callTime represent the time the function was last called
    // oldCallTime represent the last call time, callTime - oldCallTime to see 
    // time elapsed between each call.
    private float callTime = 0;
    private float oldCallTime;

    void Start() {
        panelImage = GetComponent<Image>();
        oldCallTime = Time.time;
    }

    /// <summary>
    /// Fade in and out on beatPerSond time
    /// </summary>
    /// <param name="beatPerSecond"></param>
    public void Flash(double nextTick) {
        Fade(1, 1);
        callTime = Time.time;

        fadingDuration = (callTime - oldCallTime) / 2;
        StartCoroutine(FadeOut());

        oldCallTime = callTime;
    }

    IEnumerator FadeOut() {
        if (fadingDuration == 0) fadingDuration = 0.2f;

        coStartedTime = Time.time;
        coElapsedTime = 0;

        while (coElapsedTime <= fadingDuration) {
            coElapsedTime = Time.time - coStartedTime;
            Fade(0.1f, coElapsedTime / fadingDuration);
            yield return null;
        }

        yield break;
    }

    private void Fade(float fadeTo, float fadeRatio) {
        imageColor = panelImage.color;
        imageColor.a = Mathf.Lerp(imageColor.a, fadeTo, fadeRatio);

        panelImage.color = imageColor;
    }

}
