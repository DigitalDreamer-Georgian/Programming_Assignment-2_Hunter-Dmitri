using UnityEngine;
using System.Collections;

public class dragonai : MonoBehaviour
{
    public DragonHealth dragonhealth;
    private Animator animator;
    public Transform Firepoint;
    public float speed;
    public Transform target;
    public float proximityDistance;
    public GameObject bulletPrefab;
    public float cooldown;
    public float stopDuration = 1.0f;
    private bool end = false;
    private bool start = false;
    public GameObject beamPrefab;
    public GameObject bigShotPrefab;
    public GameObject grenadePrefab;
    private enum AttackPattern { SingleShot, Beam, BigShot, BurstShot, Wave, Grenade }
    private AttackPattern currentAttackPattern = AttackPattern.SingleShot;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (end || !start)//Dont do anything before the boss enter the battlefield or after it is defeated
        {
            return;
        }
        if (dragonhealth.health <= dragonhealth.maxHealth / 2)
        {
            animator.SetBool("Stage2", true);
        }

        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        if (distanceToTarget <= proximityDistance && !isAttacking && start)
        {
            StartCoroutine(AttackPatternSequence());
        }

    }

    IEnumerator AttackPatternSequence()
    {
        if (end)
        {
            yield break; // Exit if the dragon is defeated
        }
        isAttacking = true;

        if (dragonhealth.health > dragonhealth.maxHealth / 2)
        {
           
            for (int i = 0; i < 4; i++)
            {
                animator.SetTrigger("Shot");
                yield return new WaitForSeconds(2f); 
            }
            yield return new WaitForSeconds(1f);
            WaveAttack();
            yield return new WaitForSeconds(2f);
            BigShot();
        }
        else
        {
            animator.SetTrigger("Shot");
            yield return new WaitForSeconds(1f);
            BurstShot();
            yield return new WaitForSeconds(2f);
            animator.SetTrigger("Shot");
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(FireBeam((target.position - Firepoint.position).normalized));
            yield return new WaitForSeconds(3f);
            GrenadeShot();
        }

        isAttacking = false;
    }

    void AimAndShoot()
    {
        Vector2 direction = (target.position - Firepoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, Firepoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    IEnumerator FireBeam(Vector2 direction)
    {
        if (end)
        {
            yield break; // Exit if the dragon is defeated
        }
        animator.SetTrigger("Shot");
        for (int i = 0; i < 15; i++)
        {
            GameObject beam = Instantiate(beamPrefab, Firepoint.position, Quaternion.identity);
            beam.GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
            Destroy(beam, 8f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void BigShot()
    {
        if (end)
        {
            return; // Exit if the dragon is defeated
        }
        animator.SetTrigger("Shot");
        Vector2 direction = (target.position - Firepoint.position).normalized;

        GameObject bigShot = Instantiate(bigShotPrefab, Firepoint.position, Quaternion.identity);
        bigShot.GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }

    void BurstShot()
    {
        if (end)
        {
            return; // Exit if the dragon is defeated
        }
        animator.SetTrigger("Shot");
        Vector2 direction = (target.position - Firepoint.position).normalized;

        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, Firepoint.position, Quaternion.identity);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + Random.Range(-10f, 10f)));
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * speed;
            }
        }
    }

    void WaveAttack()
    {
        if (end)
        {
            return; // Exit if the dragon is defeated
        }
        animator.SetTrigger("Shot");
        Vector2 direction = (target.position - Firepoint.position).normalized;
        float spreadAngle = 45f;
        float stepAngle = spreadAngle / 5;

        for (int i = 0; i < 6; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, Firepoint.position, Quaternion.identity);
            float angle = -spreadAngle / 2 + stepAngle * i;
            Vector2 spreadDirection = Quaternion.Euler(0, 0, angle) * direction;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = spreadDirection * speed;
            }
            Destroy(bullet, 3f);
        }
    }

    void GrenadeShot()
    {
        if (end)
        {
            return; // Exit if the dragon is defeated
        }
        animator.SetTrigger("Shot");
        Vector2 direction = (target.position - Firepoint.position).normalized;

        GameObject grenade = Instantiate(grenadePrefab, Firepoint.position, Quaternion.identity);
        grenade.GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }
    void StartDragon()
    {
        start = true;
    }
}

