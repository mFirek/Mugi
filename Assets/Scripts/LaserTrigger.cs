using UnityEngine;
using System.Collections;

public class LaserTrigger : MonoBehaviour
{
    public GameObject laserPrefab;  // Prefab lasera, który pojawi siê po kolizji
    public Transform laserSpawnPoint;  // Punkt, w którym pojawi siê laser
    private GameObject currentLaser;  // Aktualny obiekt lasera

    AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.GetInstance();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && currentLaser == null)  // Sprawdzenie kolizji z graczem
        {
            // Stworzenie lasera w miejscu okreœlonym przez laserSpawnPoint
            currentLaser = Instantiate(laserPrefab, laserSpawnPoint.position, Quaternion.identity);

            // Pobranie komponentu Animator i uruchomienie jego dzia³ania
            Animator laserAnimator = currentLaser.GetComponent<Animator>();

            // Uruchomienie metody, która usunie obiekt po zakoñczeniu animacji
            StartCoroutine(DestroyLaserAfterAnimation(laserAnimator));

            // Odtwarzanie dŸwiêku z opóŸnieniem
            StartCoroutine(PlayLaserSFXWithDelay(0.5f)); // 1 sekunda opóŸnienia
        }
    }

    private IEnumerator PlayLaserSFXWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.GetInstance().PlaySFX(audioManager.laserAttack);
    }

    private IEnumerator DestroyLaserAfterAnimation(Animator laserAnimator)
    {
        // Czekanie a¿ zakoñczy siê animacja
        yield return new WaitForSeconds(laserAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Zniszczenie obiektu lasera po zakoñczeniu animacji
        Destroy(currentLaser);
    }
}
