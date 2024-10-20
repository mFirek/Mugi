using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel; // Panel z dialogiem
    [SerializeField] private TextMeshProUGUI dialogueText; // Tekst dialogu

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Destroying duplicate DialogueManager instance");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("DialogueManager instance created");
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        // Obs³uga przycisku do kontynuowania dialogu
        if (Input.GetKeyDown(KeyCode.X)) 
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
    }

    public void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string text = currentStory.Continue();
            dialogueText.text = text;
        }
        else
        {
            ExitDialogueMode();
        }   
    }
}
