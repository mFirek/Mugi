using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue; // Wizualna wskaz�wka, kt�ra ma si� pojawi�

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON; // Plik JSON z dialogiem

    private bool playerInRange = false; // Flaga informuj�ca, czy gracz jest w zasi�gu dialogu

    private void Awake()
    {
        playerInRange = false; // Na pocz�tku ustawiamy, �e gracz nie jest w zasi�gu dialogu
        visualCue.SetActive(false); // Ukrywamy wizualn� wskaz�wk� na pocz�tku
    }

    private void Update()
    {
        // Sprawdzenie, czy gracz jest w zasi�gu dialogu i dialog nie jest w trakcie odtwarzania
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true); // Pokazujemy wizualn� wskaz�wk�
            if (Input.GetKeyDown(KeyCode.X))
            {
                visualCue.SetActive(false); // Ukrywamy wskaz�wk� po rozpocz�ciu dialogu
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON); // Rozpoczynamy dialog
            }
        }
        else
        {
            visualCue.SetActive(false); // Ukrywamy wskaz�wk�, gdy dialog jest odtwarzany
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // Gdy gracz wejdzie w zasi�g dialogu, ustawiamy flag� na true
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // Gdy gracz opu�ci zasi�g dialogu, ustawiamy flag� na false
            visualCue.SetActive(false); // Ukrywamy wizualn� wskaz�wk�
        }
    }
}
