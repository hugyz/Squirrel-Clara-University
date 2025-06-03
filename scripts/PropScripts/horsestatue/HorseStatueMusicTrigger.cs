using UnityEngine;

public class HorseStatueMusicTrigger : MonoBehaviour
{
    public AudioSource minecraftMusic;
    private bool isActive = false;

    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isActive)
        {
            isActive = true;

            if (minecraftMusic != null)
            {
                minecraftMusic.time = 0f; // reset playback
                GlobalMusicManager.Instance.PlayExclusiveMusic(minecraftMusic);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && isActive)
        {
            GlobalMusicManager.Instance.PlayExclusiveMusic(GlobalMusicManager.Instance.mainMusic); // resets to main
            isActive = false;
        }
    }

}
