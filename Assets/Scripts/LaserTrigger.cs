using UnityEngine;
using System.Collections;
public class LaserTrigger : MonoBehaviour
{
    public GameObject laserPrefab;  // Prefab lasera, który pojawi siê po kolizji
    public Transform laserSpawnPoint;  // Punkt, w którym pojawi siê laser
    private GameObject currentLaser;  // Aktualny obiekt lasera

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
        }
    }

    private IEnumerator DestroyLaserAfterAnimation(Animator laserAnimator)
    {
        // Czekanie a¿ zakoñczy siê animacja
        yield return new WaitForSeconds(laserAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Zniszczenie obiektu lasera po zakoñczeniu animacji
        Destroy(currentLaser);
    }
}
