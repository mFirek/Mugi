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

    private bool canContinue = true; 

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
    private IEnumerator WaitForNextLine()
    {
        canContinue = false;
        yield return new WaitForSeconds(0.1f); // Krótkie opóŸnienie, np. 0.1 sekundy
        canContinue = true;
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
        // Resetuj historiê, aby zaczynaæ dialog od pocz¹tku
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        // U¿yj ContinueStory() do wyœwietlenia pierwszej linii dialogu
        ContinueStory();
    }

    public void ExitDialogueMode()
    {
        dialogueIsPlaying = false; // Wa¿ne, aby zakoñczyæ tryb dialogu
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        Debug.Log("Dialogue mode exited.");
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string text = currentStory.Continue();
            dialogueText.text = text;
            Debug.Log("Continued story: " + text);
        }
        else
        {
            Debug.Log("Dialogue ended. Exiting dialogue mode.");
            ExitDialogueMode();
        }
    }


}
