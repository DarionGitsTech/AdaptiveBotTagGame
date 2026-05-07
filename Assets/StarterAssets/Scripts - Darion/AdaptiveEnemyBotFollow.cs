using UnityEngine;
using UnityEngine.AI;

public class AdaptiveEnemyBotFollow : MonoBehaviour
{
    public Transform target;

    [Header("Adaptive Difficulty")]
    public float easySpeed = 2.5f;
    public float mediumSpeed = 3.5f;
    public float hardSpeed = 5f;

    [Header("Tag Settings")]
    public float tagDistance = 2f;
    public float resetDelay = 1.5f;

    private NavMeshAgent agent;
    private float survivalTime = 0f;
    private bool playerTagged = false;
    private string currentDifficulty = "EASY";
    private float resetTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                target = playerObj.transform;
        }

        ResetDifficulty();
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (playerTagged)
        {
            resetTimer += Time.deltaTime;

            if (resetTimer >= resetDelay)
            {
                ResetRound();
            }

            return;
        }

        survivalTime += Time.deltaTime;

        UpdateDifficulty();

        agent.SetDestination(target.position);

        if (distance <= tagDistance)
        {
            playerTagged = true;
            resetTimer = 0f;

            agent.ResetPath();

            Debug.Log("PLAYER TAGGED!");
        }
    }

    void UpdateDifficulty()
    {
        if (survivalTime < 10f)
        {
            currentDifficulty = "EASY";
            agent.speed = easySpeed;
            agent.acceleration = 6f;
            agent.angularSpeed = 180f;
        }
        else if (survivalTime < 25f)
        {
            currentDifficulty = "MEDIUM";
            agent.speed = mediumSpeed;
            agent.acceleration = 10f;
            agent.angularSpeed = 360f;
        }
        else
        {
            currentDifficulty = "HARD";
            agent.speed = hardSpeed;
            agent.acceleration = 16f;
            agent.angularSpeed = 720f;
        }
    }

    void ResetRound()
    {
        survivalTime = 0f;
        playerTagged = false;
        resetTimer = 0f;

        ResetDifficulty();

        Debug.Log("ROUND RESET — Difficulty back to EASY");
    }

    void ResetDifficulty()
    {
        currentDifficulty = "EASY";
        agent.speed = easySpeed;
        agent.acceleration = 6f;
        agent.angularSpeed = 180f;
    }

    public float GetSurvivalTime()
    {
        return survivalTime;
    }

    public bool IsPlayerTagged()
    {
        return playerTagged;
    }

    public string GetDifficultyLevel()
    {
        return currentDifficulty;
    }
}