using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float speed;               // Movement speed
    public Transform target;          // Target to follow (e.g., the player)
    public float minimumDistance;     // Minimum distance to maintain from the target
    public float proximityDistance;   // Distance within which the object starts chasing

    public GameObject bulletPrefab;   // Bullet prefab to shoot
    public float cooldown;            // Time between shots
    private float nextshot;           // Tracks when the next shot can occur

    public float stopDuration = 1.0f; // Time to stop moving after shooting
    private bool isStopped = false;   // Flag to track if movement is stopped
    private float stopEndTime;        // Time when the bee can move again

    void Update()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // Handle stopping after shooting
        if (isStopped)
        {
            if (Time.time >= stopEndTime)
            {
                isStopped = false; // Resume movement
            }
            else
            {
                return; // Skip further logic while stopped
            }
        }

        // Check if the target is within the proximity distance
        if (distanceToTarget <= proximityDistance)
        {
            if (Time.time > nextshot)
            {
                AimAndShoot(); // Shoot and stop movement
            }

            // Move away if too close, otherwise approach
            if (distanceToTarget < minimumDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }
    }

    void AimAndShoot()
    {
        // Calculate direction to target
        Vector2 direction = (target.position - transform.position).normalized;

        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }

        // Set stop duration
        isStopped = true;
        stopEndTime = Time.time + stopDuration;

        // Set next shot time
        nextshot = Time.time + cooldown;
    }
}
