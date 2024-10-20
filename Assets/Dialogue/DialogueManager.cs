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

        // Obs�uga przycisku do kontynuowania dialogu
        if (Input.GetKeyDown(KeyCode.X))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        // Resetuj histori�, aby zaczyna� dialog od pocz�tku
        currentStory = new Story(inkJSON.text);

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        // Automatyczne pokazanie pierwszej linii dialogu od razu po wej�ciu w tryb dialogu
        ContinueStory();
    }

    public void ExitDialogueMode()
    {
        dialogueIsPlaying = false; // Wa�ne, aby zako�czy� tryb dialogu
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
        }
        else
        {
            Debug.Log("Dialogue ended. Exiting dialogue mode.");
            ExitDialogueMode();
        }
    }
}
