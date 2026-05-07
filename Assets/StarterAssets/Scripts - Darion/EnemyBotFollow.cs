using UnityEngine;
using UnityEngine.AI;

public class EnemyBotFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Chase Settings")]
    public float detectionRange = 30f;
    public float stoppingDistance = 1.5f;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        agent.stoppingDistance = stoppingDistance;
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
        }
    }
}