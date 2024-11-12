using System.Collections;
using UnityEngine;
using TMPro;

public class TimerText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    private static TimerText instance;
    private static float elapsedTime = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Nowa metoda do pobierania up³ywu czasu
    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
