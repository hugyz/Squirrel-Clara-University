using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public Button closeButton;

    public AudioSource npcMusic;

    private string[] lines;
    private int currentLineIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        dialoguePanel.SetActive(false);

        nextButton.onClick.AddListener(NextLine);
        closeButton.onClick.AddListener(EndDialogue);
    }

    public void ShowDialogue(string[] dialogueLines)
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
            return;

        lines = dialogueLines;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        dialogueText.text = lines[currentLineIndex];

        // Save and pause current music, play NPC music
        GlobalMusicManager.Instance.PlayExclusiveMusic(npcMusic);
    }

    public void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex < lines.Length)
        {
            dialogueText.text = lines[currentLineIndex];
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        lines = null;
        currentLineIndex = 0;

        // Resume music that was playing before dialogue
        GlobalMusicManager.Instance.StopExclusiveMusicAndResumePrevious();
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            EndDialogue();
        }
    }
}
