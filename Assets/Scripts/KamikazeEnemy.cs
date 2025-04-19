using UnityEngine;

public class KamikazeEnemy : MonoBehaviour
{
    public float speed = 3f;
    public Transform target;
    public float proximityDistance = 10f;
    public GameObject grenadePrefab;

    void Update()
    {
        if (target == null) return;

        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        if (distanceToTarget <= proximityDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (grenadePrefab != null)
            {
                Instantiate(grenadePrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
