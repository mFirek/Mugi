using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager1 : MonoBehaviour
{
    public GameObject[] select_level;
    void Start()
    {
        select_level[LevelSelectionMenuManager.currLevel].SetActive(true); 
    }

    void Update()
    {
        
    }
}
