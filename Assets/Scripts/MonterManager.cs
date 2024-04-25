using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject monsterPrefab; // Prefabrykat potwora
    public Transform respawnPoint; // Punkt, w którym potwór ma siê odradzaæ

    private GameObject currentMonster; // Obecny potwór w grze
    private bool playerInStartArea = false; // Czy gracz jest w obszarze startowym

    void OnTriggerEnter2D(Collider2D other)
    {
        // SprawdŸ, czy gracz wszed³ w obszar startowy
        if (other.CompareTag("Player"))
        {
            playerInStartArea = true;
            RespawnMonsterIfNecessary();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // SprawdŸ, czy gracz opuœci³ obszar startowy
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
        // SprawdŸ, czy gracz jest w obszarze startowym i czy obecny potwór jest martwy lub nieaktywny
        if (playerInStartArea && (currentMonster == null || !currentMonster.activeSelf))
        {
            // Jeœli obecny potwór istnieje, ale jest nieaktywny, ustaw go jako aktywny
           
            // Jeœli obecny potwór nie istnieje, stwórz nowego potwora na punkcie respawnu
          
                currentMonster = Instantiate(monsterPrefab, respawnPoint.position, Quaternion.identity);
            
        }
    }

    // Metoda wywo³ywana, gdy potwór zostanie zniszczony lub wy³¹czony
    public void MonsterDestroyedOrDeactivated(GameObject monster)
    {
        // SprawdŸ, czy zniszczony lub wy³¹czony potwór jest obecnym potworem
        if (monster == currentMonster)
        {
            currentMonster = null; // Ustaw obecny potwór na null, aby umo¿liwiæ zrespawnowanie
        }
    }
}
