using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue; // Wizualna wskazówka, która ma siê pojawiæ

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON; // Plik JSON z dialogiem

    private bool playerInRange = false; // Flaga informuj¹ca, czy gracz jest w zasiêgu dialogu

    private void Awake()
    {
        playerInRange = false; // Na pocz¹tku ustawiamy, ¿e gracz nie jest w zasiêgu dialogu
        visualCue.SetActive(false); // Ukrywamy wizualn¹ wskazówkê na pocz¹tku
    }

    private void Update()
    {
        // Sprawdzenie, czy gracz jest w zasiêgu dialogu i dialog nie jest w trakcie odtwarzania
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true); // Pokazujemy wizualn¹ wskazówkê
            if (Input.GetKeyDown(KeyCode.X))
            {
                visualCue.SetActive(false); // Ukrywamy wskazówkê po rozpoczêciu dialogu
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON); // Rozpoczynamy dialog
            }
        }
        else
        {
            visualCue.SetActive(false); // Ukrywamy wskazówkê, gdy dialog jest odtwarzany
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // Gdy gracz wejdzie w zasiêg dialogu, ustawiamy flagê na true
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // Gdy gracz opuœci zasiêg dialogu, ustawiamy flagê na false
            visualCue.SetActive(false); // Ukrywamy wizualn¹ wskazówkê
        }
    }
}
