using System.Collections;
using UnityEngine;

// wrzucić do istniejącego character controlera

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour, IDataPresistence
{
    public void LoadData(GameData gameData)
    {
        //this.transform.position = DataPresistenceManager.playerPosition;
        //wczytuje pozycję gracza
    }

    public void SaveData(ref GameData gameData) 
    {
        //data.playerPosition = this.transform.position;
        //zapisuje pozycję gracza
    }

}