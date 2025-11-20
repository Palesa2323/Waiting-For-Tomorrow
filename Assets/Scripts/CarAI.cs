using UnityEngine;

public class CarAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5f;
    public float turnSpeed = 5f;

    private int currentWaypointIndex = 0;

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        Transform target = waypoints[currentWaypointIndex];

        // Direction
        Vector3 direction = (target.position - transform.position).normalized;

        // Move only on XZ
        Vector3 move = new Vector3(direction.x, 0, direction.z) * speed * Time.deltaTime;
        transform.position += move;

        // Rotate smoothly
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        // Check if reached waypoint
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                             new Vector3(target.position.x, 0, target.position.z)) < 0.5f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 0;
        }
    }


}
