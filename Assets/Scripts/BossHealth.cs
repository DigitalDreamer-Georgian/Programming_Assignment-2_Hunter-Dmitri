using UnityEngine;

public class BossHealth : Enemy
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject grenadePrefab;

    [SerializeField] private float cooldown = 2f;
    private float nextShotTime;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float minimumDistance = 2f;
    [SerializeField] private float proximityDistance = 10f;
    [SerializeField] private int extraGrenades = 1;

    private bool isStopped = false;
    private float stopDuration = 1f;
    private float stopEndTime;

    private bool useGrenade = false;

    void Update()
    {
        // Health/death logic from base
        if (health <= 0)
        {
            health = 0;
            Dead();
            return;
        }

        if (health <= maxHealth / 2)
        {
            useGrenade = true;
        }

        if (target == null) return;

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (isStopped)
        {
            if (Time.time >= stopEndTime)
            {
                isStopped = false;
            }
            else
            {
                return;
            }
        }

        if (distanceToTarget <= proximityDistance)
        {
            if (Time.time >= nextShotTime)
            {
                ShootAtPlayer();
            }

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

    private void ShootAtPlayer()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        ShootProjectile(direction, useGrenade ? grenadePrefab : bulletPrefab);

        if (useGrenade)
        {
            for (int i = 0; i < extraGrenades; i++)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                ShootProjectile(randomDir, grenadePrefab);
            }
        }

        isStopped = true;
        stopEndTime = Time.time + stopDuration;
        nextShotTime = Time.time + cooldown;
    }

    private void ShootProjectile(Vector2 direction, GameObject prefab)
    {
        GameObject proj = Instantiate(prefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }
}
