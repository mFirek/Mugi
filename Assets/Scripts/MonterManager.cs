using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject monsterPrefab; // Prefabrykat potwora
    public Transform respawnPoint; // Punkt, w kt�rym potw�r ma si� odradza�

    private GameObject currentMonster; // Obecny potw�r w grze
    private bool playerInStartArea = false; // Czy gracz jest w obszarze startowym

    void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawd�, czy gracz wszed� w obszar startowy
        if (other.CompareTag("Player"))
        {
            playerInStartArea = true;
            RespawnMonsterIfNecessary();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Sprawd�, czy gracz opu�ci� obszar startowy
        if (other.CompareTag("Player"))
        {
            playerInStartArea = false;
        }
    }

    void RespawnMonsterIfNecessary()
    {
        if (currentMonster != null)
        {
            gameObject.SetActive(true);
        }
        // Sprawd�, czy gracz jest w obszarze startowym i czy obecny potw�r jest martwy lub nieaktywny
        if (playerInStartArea && (currentMonster == null || !currentMonster.activeSelf))
        {
            // Je�li obecny potw�r istnieje, ale jest nieaktywny, ustaw go jako aktywny
           
            // Je�li obecny potw�r nie istnieje, stw�rz nowego potwora na punkcie respawnu
          
                currentMonster = Instantiate(monsterPrefab, respawnPoint.position, Quaternion.identity);
            
        }
    }

    // Metoda wywo�ywana, gdy potw�r zostanie zniszczony lub wy��czony
    public void MonsterDestroyedOrDeactivated(GameObject monster)
    {
        // Sprawd�, czy zniszczony lub wy��czony potw�r jest obecnym potworem
        if (monster == currentMonster)
        {
            currentMonster = null; // Ustaw obecny potw�r na null, aby umo�liwi� zrespawnowanie
        }
    }
}
