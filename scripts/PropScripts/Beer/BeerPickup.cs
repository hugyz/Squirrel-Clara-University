using UnityEngine;
using System.Collections;

public class BeerPickup : MonoBehaviour
{
    public float drunkDuration = 10f;
    public float respawnTime = 20f;

    public AudioSource drunkMusic;
    public AudioSource collectSound;

    private bool isCollected = false;
    private ScreenFader screenFader;
    private SquirrelMovement squirrel;

    void Start()
    {
        screenFader = FindObjectOfType<ScreenFader>();
        squirrel = FindObjectOfType<SquirrelMovement>();
        ShowBeer(true);
    }

    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isCollected)
        {
            isCollected = true;
            StartCoroutine(PickupSequence());
            ShowBeer(false);
        }
    }

    IEnumerator PickupSequence()
    {
        // Play drunk music over whatever is currently playing
        GlobalMusicManager.Instance.PlayExclusiveMusic(drunkMusic);

        if (collectSound != null)
            collectSound.Play();

        if (screenFader != null)
            screenFader.StartFlashing(drunkDuration);

        yield return new WaitWhile(() => collectSound != null && collectSound.isPlaying);

        if (squirrel != null)
            squirrel.isDrunk = true;

        yield return new WaitForSeconds(drunkDuration);

        if (squirrel != null)
            squirrel.isDrunk = false;

        // Restore previous music from saved time
        GlobalMusicManager.Instance.StopExclusiveMusicAndResumePrevious();

        yield return new WaitForSeconds(respawnTime);
        isCollected = false;
        ShowBeer(true);
    }

    void ShowBeer(bool show)
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = show;

        foreach (Collider c in GetComponentsInChildren<Collider>())
            c.enabled = show;
    }
}
