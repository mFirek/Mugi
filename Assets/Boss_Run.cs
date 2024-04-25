using System.Collections;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    private MonoBehaviour monoBehaviour = null;

    public Boss_Run(MonoBehaviour monoBehaviour)
    {
        this.monoBehaviour = monoBehaviour;
    }

    public float speed = 2.5f;
    public float attackRange = 3f;
    public float RangeShoot = 30f;
    public float rangeShootCooldown = 3f;
    public float rangeShootTimer = 0f;
    private float cooldownTimer = 0f;
    private bool cooldownActive = false;

    public Transform firePoint;
    public GameObject bulletPrefab;

    Transform player;
    Rigidbody2D rb;
    Boss boss;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.monoBehaviour.StartCoroutine(WaitSomeThenResetStuff(animator));
    }

    private IEnumerator WaitSomeThenResetStuff(Animator animator)
    {
        yield return new WaitForSeconds(1.0f);

        rangeShootTimer = 0f;
        cooldownActive = false;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
        firePoint = GameObject.FindGameObjectWithTag("FirePoint").transform;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();

        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        rangeShootTimer += Time.deltaTime;

        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
            cooldownActive = true;
            cooldownTimer = 0f;
        }
        else if (rangeShootTimer >= rangeShootCooldown)
        {
            animator.SetTrigger("Shoot");
            Shoot();
            rangeShootTimer = 0f;
            cooldownActive = true;
        }
        if (cooldownActive)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= rangeShootCooldown)
            {
                cooldownActive = false;
            }
        }
    }

    public void Shoot()
    {
        Vector2 lookDir = player.position - firePoint.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 360f;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        if (Vector2.Distance(player.position, rb.position) <= RangeShoot)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            rbBullet.AddForce(firePoint.up * 0.2f, ForceMode2D.Impulse);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Shoot");
    }
}
