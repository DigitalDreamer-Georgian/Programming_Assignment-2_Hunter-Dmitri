using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int dmg = 1;
    public LayerMask whatIsPlayer;  // Layer mask for the player
    void Start()
    {
        Destroy(gameObject, 3f); // Destroys the GameObject this script is attached to after 3 seconds
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet collides with the player
        if (((1 << collision.gameObject.layer) & whatIsPlayer) != 0)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(dmg);  // Apply damage to the player
            }

            Destroy(gameObject);  // Destroy the bullet
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}

