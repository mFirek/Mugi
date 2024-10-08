using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText; // Text to display time
    private static TimerText instance; // Singleton for the timer
    private static float elapsedTime = 0f;  // Static time to be stored between scenes

    private void Awake()
    {
        // Make sure that only one instance of the TimerText object exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Preserve the object between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy the duplicate object
        }
    }

    private void Update()
    {
        // Add elapsed time since last frame
        elapsedTime += Time.deltaTime;

        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        // Display the time in “MM:SS” format
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
