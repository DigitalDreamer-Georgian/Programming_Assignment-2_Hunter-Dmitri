using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public LayerMask whatIsEnemies;
    void Start()
    {
        Destroy(gameObject, 3f); // Destroys the GameObject this script is attached to after 3 seconds
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsEnemies) != 0)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
