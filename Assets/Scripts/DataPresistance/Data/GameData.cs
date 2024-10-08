using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int deathCount;
          
    public int globalDeathCount;   

    public static Vector3 playerPosition;


    //values defined in this constructor will be the default values
    //game will start when there is no data to load
    public GameData()
    {
        this.deathCount = 0;
        playerPosition = Vector3.zero;
        
    }
}
