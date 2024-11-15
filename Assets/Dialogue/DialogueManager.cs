using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.04f;
    [SerializeField] private float skipHoldDuration = 2.0f;  // czas przytrzymania Space do pominiêcia ca³ego dialogu
    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private Coroutine displayLineCoroutine;
    private static DialogueManager instance;
    private float holdTime = 0f;  // czas przytrzymania Space
    private bool skipTriggered = false;  // flaga dla jednorazowego wy³¹czenia

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
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
            holdTime = 0f;
            skipTriggered = false;
            return;
        }

        // Przewijanie dialogu przyciskiem X
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.F))
        {
            ContinueStory();
        }

        // Sprawdzanie przytrzymania Space dla pominiêcia dialogu
        if (Input.GetKey(KeyCode.Space))
        {
            holdTime += Time.deltaTime;

            if (holdTime >= skipHoldDuration && !skipTriggered)
            {
                skipTriggered = true;  // flaga dla jednorazowego wy³¹czenia
                StartCoroutine(ExitDialogueMode());
            }
        }
        else
        {
            holdTime = 0f;
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
