using UnityEngine;
using System.Collections;

public class StarPickup : MonoBehaviour
{
    public float boostMultiplier = 5f;
    public float boostDuration = 10f;
    public float respawnTime = 20f;

    public AudioSource boostAudioSource;
    public AudioSource collectSoundSource;

    private SquirrelMovement squirrel;
    private float originalWalkSpeed;
    private float originalRunSpeed;
    private float originalJumpHeight;

    private bool isCollected = false;
    private bool boostActive = false;

    private MeshRenderer meshRenderer;
    private Collider col;
    private GameObject starStream;

    void Start()
    {
        squirrel = FindObjectOfType<SquirrelMovement>();
        if (squirrel != null)
        {
            Transform streamTransform = squirrel.transform.Find("StarStream");
            if (streamTransform != null)
            {
                starStream = streamTransform.gameObject;
                starStream.SetActive(false);
            }
        }

        if (collectSoundSource != null)
            collectSoundSource.Stop();

        if (boostAudioSource != null)
            boostAudioSource.Stop();

        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
        ShowStar(true);
    }

    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isCollected)
        {
            isCollected = true;
            StartCoroutine(ApplyBoostSequence());
            ShowStar(false);
        }
    }

    IEnumerator ApplyBoostSequence()
    {
        if (squirrel == null || boostActive) yield break;
        boostActive = true;

        if (collectSoundSource != null)
            collectSoundSource.Play();

        yield return new WaitWhile(() => collectSoundSource != null && collectSoundSource.isPlaying);

        // Play boost music over current track (Minecraft or Main)
        GlobalMusicManager.Instance.PlayExclusiveMusic(boostAudioSource);

        originalWalkSpeed = squirrel.walkSpeed;
        originalRunSpeed = squirrel.runSpeed;
        originalJumpHeight = squirrel.jumpHeight;

        squirrel.walkSpeed *= boostMultiplier;
        squirrel.runSpeed *= boostMultiplier;
        squirrel.jumpHeight *= boostMultiplier;

        if (starStream != null)
            starStream.SetActive(true);

        yield return new WaitForSeconds(boostDuration);

        squirrel.walkSpeed = originalWalkSpeed;
        squirrel.runSpeed = originalRunSpeed;
        squirrel.jumpHeight = originalJumpHeight;

        if (starStream != null)
            starStream.SetActive(false);

        // Resume the previously playing track from correct time
        GlobalMusicManager.Instance.StopExclusiveMusicAndResumePrevious();

        boostActive = false;

        yield return new WaitForSeconds(respawnTime);
        ResetStar();
    }

    void ShowStar(bool show)
    {
        if (meshRenderer != null)
            meshRenderer.enabled = show;

        if (col != null)
            col.enabled = show;
    }

    void ResetStar()
    {
        isCollected = false;
        ShowStar(true);
    }
}
