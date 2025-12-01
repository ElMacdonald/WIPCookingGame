using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;

    void Awake()
    {
        canvasGroup.alpha = 0f;
    }

    public IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    public IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }

    public IEnumerator FadeTransition(System.Action midAction)
    {
        yield return FadeIn();   // fade to black
        midAction?.Invoke();     // swap UI/cameras/objects
        yield return FadeOut();  // fade back to transparent
    }
}
