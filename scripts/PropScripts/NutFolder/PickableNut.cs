using UnityEngine;
using TMPro;

public class PickableNut : MonoBehaviour
{
    public Camera cam;
    public TextMeshProUGUI nutCounterText;
    public float pickupRange = 3f;
    public float respawnTime = 5f;

    private static int nutCount = 0;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isCollected = false;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        originalPosition = transform.position;
        originalRotation = transform.rotation;
        ShowNut(true);
    }

    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isCollected)
        {
            isCollected = true;
            nutCount++;

            if (nutCounterText != null)
                nutCounterText.text = "Nuts: " + nutCount;

            StartCoroutine(RespawnNut());
        }
    }

    System.Collections.IEnumerator RespawnNut()
    {
        ShowNut(false);
        yield return new WaitForSeconds(respawnTime);
        ResetNut();
        ShowNut(true);
    }

    void ShowNut(bool show)
    {
        GetComponent<Renderer>().enabled = show;
        GetComponent<Collider>().enabled = show;
        isCollected = !show;
    }

    void ResetNut()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}
