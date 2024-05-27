using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int deathCount;

    public static Vector3 playerPosition;

   
    //warto�ci zdefiniowane w tym konstruktorze b�d� domy�lnymi warto�ciami
    //gra uruchomi si� gdy nie b�dzie �adnych danych do wczytania
    public GameData()
    {
        this.deathCount = 0;
        playerPosition = Vector3.zero;
        
    }
}
