using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public CanvasGroup fadeGroup;
    public float initialFlashDuration = 3f;

    private Coroutine flashRoutine;

    void Start()
    {
        if (fadeGroup != null)
            fadeGroup.alpha = 0f; // Always hidden at start
    }

    public void StartFlashing(float totalDuration)
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashIncreasing(totalDuration));
    }

    IEnumerator FlashIncreasing(float totalDuration)
    {
        float elapsed = 0f;
        float flashDuration = initialFlashDuration;

        while (elapsed < totalDuration)
        {
            yield return StartCoroutine(FlashOnce(flashDuration));
            elapsed += flashDuration;
            flashDuration *= 1.2f; // ⬆️ increase flash length over time
        }

        fadeGroup.alpha = 0f; // 🧼 Clear screen after flashing
    }

    IEnumerator FlashOnce(float duration)
    {
        float half = duration / 2f;
        float t = 0f;

        // Fade in
        while (t < half)
        {
            fadeGroup.alpha = Mathf.Lerp(0f, 1f, t / half);
            t += Time.deltaTime;
            yield return null;
        }

        fadeGroup.alpha = 1f;
        t = 0f;

        // Fade out
        while (t < half)
        {
            fadeGroup.alpha = Mathf.Lerp(1f, 0f, t / half);
            t += Time.deltaTime;
            yield return null;
        }

        fadeGroup.alpha = 0f;
    }
}
