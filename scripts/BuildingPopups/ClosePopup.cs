using UnityEngine;

public class ClosePopup : MonoBehaviour
{
    public GameObject infoPanel;

    public void ClosePanel()
    {
        infoPanel.SetActive(false);
    }
}
