using UnityEngine;
using TMPro; // Add this line

public class BuildingInfo : MonoBehaviour
{
    [TextArea]
    public string buildingInfo;

    public GameObject infoPanel;
    public TMP_Text infoText; // Use TMP_Text instead of Text

    void OnMouseDown()
    {
        infoPanel.SetActive(true);
        infoText.text = buildingInfo;
    }
}
