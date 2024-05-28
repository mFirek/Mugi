using UnityEngine;

public class BossShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Ensure firePoint is correctly assigned
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
        }

        if (firePoint == null)
        {
            Debug.LogError("FirePoint not found");
        }
    }

    public void ShootBullet()
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint not assigned");
            return;
        }

        Vector2 lookDir = player.position - firePoint.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log("Shooting from: " + firePoint.position); // Debugging position

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        rbBullet.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse); // Adjusted force for better shooting
    }
}
