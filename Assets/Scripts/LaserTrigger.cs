using UnityEngine;
using System.Collections;
public class LaserTrigger : MonoBehaviour
{
    public GameObject laserPrefab;  // Prefab lasera, kt�ry pojawi si� po kolizji
    public Transform laserSpawnPoint;  // Punkt, w kt�rym pojawi si� laser
    private GameObject currentLaser;  // Aktualny obiekt lasera

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && currentLaser == null)  // Sprawdzenie kolizji z graczem
        {
            // Stworzenie lasera w miejscu okre�lonym przez laserSpawnPoint
            currentLaser = Instantiate(laserPrefab, laserSpawnPoint.position, Quaternion.identity);

            // Pobranie komponentu Animator i uruchomienie jego dzia�ania
            Animator laserAnimator = currentLaser.GetComponent<Animator>();

            // Uruchomienie metody, kt�ra usunie obiekt po zako�czeniu animacji
            StartCoroutine(DestroyLaserAfterAnimation(laserAnimator));
        }
    }

    private IEnumerator DestroyLaserAfterAnimation(Animator laserAnimator)
    {
        // Czekanie a� zako�czy si� animacja
        yield return new WaitForSeconds(laserAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Zniszczenie obiektu lasera po zako�czeniu animacji
        Destroy(currentLaser);
    }
}
