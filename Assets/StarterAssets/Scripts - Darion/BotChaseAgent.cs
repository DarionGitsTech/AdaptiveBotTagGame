using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BotChaseAgent : Agent
{
    public Transform target;

    public float moveSpeed = 3f;
    public float rotationSpeed = 180f;
    public float tagDistance = 1.5f;

    private float previousDistanceToTarget;

    private Rigidbody rb;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0f, 1f, 0f);
        transform.localRotation = Quaternion.identity;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        target.localPosition = new Vector3(
            Random.Range(-2f, 2f), // orig. 4f
            0.5f,
            Random.Range(-2f, 2f) // orig. 4f
        );

        previousDistanceToTarget =
            Vector3.Distance(transform.localPosition, target.localPosition);
    }

    public override void CollectObservations(VectorSensor sensor)
    {

        Debug.Log("CollectObservations is running");

        Vector3 toTarget = target.localPosition - transform.localPosition;

        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.z);

        sensor.AddObservation(target.localPosition.x);
        sensor.AddObservation(target.localPosition.z);

        sensor.AddObservation(toTarget.normalized.x);
        sensor.AddObservation(toTarget.normalized.z);

        sensor.AddObservation(toTarget.magnitude);

        sensor.AddObservation(transform.forward.x);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log("Action received");

        float moveInput = actions.ContinuousActions[0];
        float rotateInput = actions.ContinuousActions[1];

        transform.position += transform.forward * moveInput * moveSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotateInput * rotationSpeed * Time.deltaTime);

        float currentDistanceToTarget =
            Vector3.Distance(transform.localPosition, target.localPosition);

        // Small time penalty
        AddReward(-0.001f);

        // Reward moving closer
        if (currentDistanceToTarget < previousDistanceToTarget)
        {
            AddReward(0.01f);
        }
        else
        {
            // Punish moving farther away
            AddReward(-0.01f);
        }

        // Update stored distance
        previousDistanceToTarget = currentDistanceToTarget;

        // Big reward for tagging target
        if (currentDistanceToTarget <= tagDistance)
        {
            AddReward(1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;

        continuousActions[0] = Input.GetAxis("Vertical");
        continuousActions[1] = Input.GetAxis("Horizontal");
    }
}