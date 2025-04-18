using UnityEngine;

public class Melee : MonoBehaviour
{
    public Animator animator;
    public float speed;               // Movement speed
    public Transform target;          // Target to follow (e.g., the player)
    public float attackRange;         // Distance at which the enemy attacks
    public float attackCooldown = 1f; // Time between attacks
    public int damage = 10;           // Damage dealt to the player

    private float nextAttackTime = 0f; // Tracks when the next attack can occur

    public float minimumDistance;     // Minimum distance to maintain from the target
    public float proximityDistance;   // Distance within which the object starts chasing

    void Update()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
        }
        else
        {
            Followplayer();
        }
    }



    void Attack()
    {
        nextAttackTime = Time.time + attackCooldown; // Set cooldown
        Debug.Log("Enemy attacks!");
        if(animator != null)
        {
            animator.SetTrigger("Attack");
        }
        if (target.CompareTag("Player"))
        {
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    void Followplayer()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // Check if the target is within the proximity distance and greater than the minimum distance
        if (distanceToTarget <= proximityDistance && distanceToTarget > minimumDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}

