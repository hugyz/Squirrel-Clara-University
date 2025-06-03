using UnityEngine;
using System.Collections;

public class GlobalMusicManager : MonoBehaviour
{
    public static GlobalMusicManager Instance;

    public AudioSource mainMusic;

    private AudioSource currentMusic;
    private AudioSource previousMusic;
    private float previousTime;

    private Coroutine transitionRoutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (mainMusic != null)
        {
            currentMusic = mainMusic;
            currentMusic.Play();
        }
    }

    public void PlayExclusiveMusic(AudioSource newMusic)
    {
        if (newMusic == null || newMusic == currentMusic)
            return;

        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        transitionRoutine = StartCoroutine(HandleMusicSwitch(newMusic, resumePrevious: false));
    }

    public void StopExclusiveMusicAndResumePrevious()
    {
        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        transitionRoutine = StartCoroutine(HandleMusicSwitch(previousMusic, resumePrevious: true));
    }

    private IEnumerator HandleMusicSwitch(AudioSource newMusic, bool resumePrevious)
    {
        // Pause current music and store it for resuming
        if (currentMusic != null && currentMusic.isPlaying)
        {
            if (!resumePrevious)
            {
                // Store the current as previous no matter what (even if it's mainMusic)
                previousMusic = currentMusic;
                previousTime = currentMusic.time;
            }

            currentMusic.Pause();
        }

        yield return new WaitForSeconds(0.5f);

        currentMusic = newMusic;

        if (currentMusic != null)
        {
            if (resumePrevious)
                currentMusic.time = previousTime;

            currentMusic.Play();
        }
    }

    // Optional: Force resume to mainMusic
    public void ResumeMainMusic()
    {
        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        transitionRoutine = StartCoroutine(HandleMusicSwitch(mainMusic, resumePrevious: false));
    }
}
