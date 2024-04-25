using Pathfinding;
using UnityEngine;

public class FlyingEnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public string playerTag = "Player"; // Tag gracza
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 5f;

    [Header("Custom Behavior")]
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Transform target; // Transformacja gracza

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        // Znajd� transformacj� gracza na podstawie tagu
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Nie mo�na znale�� obiektu z tagiem " + playerTag);
        }

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if (TargetInDistance())
        {
            MoveTowardsTarget();
        }
    }

    private void UpdatePath()
    {
        if (TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void MoveTowardsTarget()
    {
        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // Obliczenie kierunku
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;

        // Ruch
        rb.MovePosition(rb.position + force * Time.fixedDeltaTime);

        // Sprawdzenie, czy osi�gni�to punkt kontrolny
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < 0.1f)
        {
            currentWaypoint++;
        }

        // Obs�uga grafiki kierunku
        if (directionLookEnabled)
        {
            Vector2 directionToTarget = target.position - transform.position;
            if (directionToTarget.x > 0)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (directionToTarget.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
