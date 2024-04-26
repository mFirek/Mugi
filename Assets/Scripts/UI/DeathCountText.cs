using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCountText : MonoBehaviour, IDataPresistence
{
    private int deathCount = 0;

    private TextMeshProUGUI deathCountText;

    private void Awake() 
    {
        deathCountText = this.GetComponent<TextMeshProUGUI>();
    }

    public void LoadData(GameData gameData)
    {
        this.deathCount = gameData.deathCount;
    }

    public void SaveData(ref GameData data)
    {
        this.deathCount = data.deathCount;
    }

    private void Start() 
    {
        // subscribe to events
        GameEventsManager.instance.onPlayerDeath += OnPlayerDeath;
    }

    private void OnDestroy() 
    {
        // unsubscribe from events
        GameEventsManager.instance.onPlayerDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath() 
    {
        deathCount++;
    }

    private void Update() 
    {
        deathCountText.text = "" + deathCount;
    }
}
