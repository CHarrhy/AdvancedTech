using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour
{
    public int numberOfWaypoints = 10; // Adjust the number of waypoints as needed
    public float patrolRange = 30f; // Adjust the range within which waypoints are generated
    public float moveSpeed = 3f;
    public float followDistance = 5f;
    public float followAngle = 180f;

    private int currentWaypointIndex = 0;
    private Transform player;
    private bool isFollowingPlayer = false;
    private NavMeshAgent navMeshAgent;

    // Use a List<Vector3> to store dynamically generated waypoints
    private List<Vector3> waypoints = new List<Vector3>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            Debug.LogError("Player not found. Make sure to tag the player GameObject with 'Player'.");
        }

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found on the enemy GameObject.");
        }
        else
        {
            SetAgentSettings();
            GenerateRandomWaypoints();
            SetDestinationToWaypoint();
        }
    }

    void SetAgentSettings()
    {
        // Set additional NavMesh Agent settings for obstacle avoidance
        navMeshAgent.radius = 0.5f; // Adjust according to your agent's size
        navMeshAgent.height = 2.0f; // Adjust according to your agent's size
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        navMeshAgent.avoidancePriority = 50; // Higher priority to favor avoidance
    }

    void GenerateRandomWaypoints()
    {
        // Generate random waypoints within the specified range
        waypoints.Clear(); // Clear existing waypoints

        for (int i = 0; i < numberOfWaypoints; i++)
        {
            waypoints.Add(new Vector3(
                Random.Range(transform.position.x - patrolRange, transform.position.x + patrolRange),
                transform.position.y,
                Random.Range(transform.position.z - patrolRange, transform.position.z + patrolRange)
            ));
        }
    }

    void Update()
    {
        if (player != null)
        {
            if (IsPlayerInFront() && IsPlayerInRange(followDistance) && !IsPlayerHiddenBehindObstacle())
            {
                isFollowingPlayer = true;
            }

            if (isFollowingPlayer)
            {
                FollowPlayer();

                if (!IsPlayerInRange(followDistance) || IsPlayerHiddenBehindObstacle())
                {
                    isFollowingPlayer = false;
                    GenerateRandomWaypoints(); // Generate new waypoints
                    SetDestinationToWaypoint();
                }
            }
            else
            {
                Patrol();
            }
        }
    }

    void Patrol()
    {
        if (waypoints.Count == 0)
        {
            Debug.LogError("No waypoints assigned to the enemy AI.");
            return;
        }

        if (navMeshAgent.isActiveAndEnabled && navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance < 0.1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
                SetDestinationToWaypoint();
            }
        }
        else
        {
            Debug.LogError("NavMeshAgent is not active or has an invalid path.");
        }
    }

    void FollowPlayer()
    {
        if (navMeshAgent.isActiveAndEnabled)
        {
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            Debug.LogError("NavMeshAgent is not active.");
        }
    }

    void SetDestinationToWaypoint()
    {
        if (waypoints.Count > 0 && navMeshAgent.isActiveAndEnabled)
        {
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex]);
        }
    }

    bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.position) < range;
    }

    bool IsPlayerInFront()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        return Mathf.Abs(angle) < followAngle / 2f;
    }

    bool IsPlayerHiddenBehindObstacle()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, followDistance))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                return true;
            }
        }

        return false;
    }
}