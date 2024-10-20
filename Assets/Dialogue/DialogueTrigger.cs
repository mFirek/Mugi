using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange = false;
    private bool dialogueStarted = false; // Flaga do kontroli czy dialog ju¿ siê rozpocz¹³

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        // Sprawdzenie, czy gracz jest w zasiêgu dialogu i dialog nie jest odtwarzany
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying && !dialogueStarted)
        {
            visualCue.SetActive(true);

            if (Input.GetKeyDown(KeyCode.X))
            {
                visualCue.SetActive(false);
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                dialogueStarted = true; // Zablokowanie ponownego uruchomienia dialogu
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            visualCue.SetActive(false);
            dialogueStarted = false; // Resetowanie flagi po opuszczeniu zasiêgu
        }
    }
}
