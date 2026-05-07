using UnityEngine;

public class BotTagDetector : MonoBehaviour
{
    public Transform player;

    public float tagDistance = 2f;

    private bool playerTagged = false;

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
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= tagDistance)
        {
            if (!playerTagged)
            {
                playerTagged = true;
                Debug.Log("PLAYER IS BEING TAGGED!");
            }
        }
        else
        {
            if (playerTagged)
            {
                playerTagged = false;
                Debug.Log("PLAYER IS NO LONGER BEING TAGGED!");
            }
        }
    }
}