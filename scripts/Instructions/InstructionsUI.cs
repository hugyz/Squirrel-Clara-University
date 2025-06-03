using UnityEngine;

public class InstructionsUI : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject titleText;
    public GameObject backgroundImage;

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
        titleText.SetActive(false);
        backgroundImage.SetActive(false);
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
        titleText.SetActive(true);
        backgroundImage.SetActive(true);
    }
}
