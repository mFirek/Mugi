using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;
    private bool dialogueCompleted; // Flaga informuj¹ca, czy dialog zosta³ zakoñczony

    private void Awake()
    {
        playerInRange = false;
        dialogueCompleted = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        // Pokazanie wizualnego wskaŸnika, jeœli gracz jest w zasiêgu i dialog nie jest aktywny
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying && !dialogueCompleted)
        {
            visualCue.SetActive(true);

            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.F))
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                dialogueCompleted = true; // Oznaczenie, ¿e dialog siê rozpocz¹³
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Sprawdzenie, czy gracz mo¿e wejœæ w dialog
            if (!dialogueCompleted)
            {
                playerInRange = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;

            // Resetowanie flagi po opuszczeniu zasiêgu
            dialogueCompleted = false;
        }
    }
}
