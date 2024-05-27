using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int deathCount;

    public static Vector3 playerPosition;

   
    //wartoœci zdefiniowane w tym konstruktorze bêd¹ domyœlnymi wartoœciami
    //gra uruchomi siê gdy nie bêdzie ¿adnych danych do wczytania
    public GameData()
    {
        this.deathCount = 0;
        playerPosition = Vector3.zero;
        
    }
}
