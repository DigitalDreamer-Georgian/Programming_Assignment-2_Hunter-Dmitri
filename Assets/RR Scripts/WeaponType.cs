
using UnityEngine;

public enum WeaponType
{
    SWORD,
    GUN,
    SHOTGUN,
    COUNT
}

public class Player : MonoBehaviour
{
    public Animator ani;
    public GameObject GunSprite;
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    GameObject bulletsPrefab;

    float bulletSpeed = 10.0f;

    public WeaponType weaponType = WeaponType.GUN;

    [SerializeField]
    Transform attackPos;

    [SerializeField]
    LayerMask whatIsEnemies;
    float attackRange = 1.0f;
    int damage = 1;
    float startTimeBtwAttack = 0.5f;
    float timeBtwAttack;

    // Ammo counts and reload times
    int pistolAmmo = 12;
    int shotgunAmmo = 6;
    float pistolCooldown = 0.1f;  // Cooldown between shots for pistol
    float shotgunCooldown = 0.4f; // Cooldown between shots for shotgun
    float reloadCooldownPistol = 0.5f;  // 1 second reload for pistol
    float reloadCooldownShotgun = 0.8f;  // 2 seconds reload for shotgun
    float timeSinceLastShot;
    float reloadTimeRemaining = 0f;

    void Update()
    {
        // Aiming
        Vector3 mouse = Input.mousePosition;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        mouse.z = 0.0f;
        Vector3 mouseDirection = (mouse - transform.position).normalized;
        Debug.DrawLine(transform.position, transform.position + mouseDirection * 5.0f);
        // Calculate the angle in degrees
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        GunSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        GunSprite.transform.position = transform.position + mouseDirection * 0.7f;
        if (weaponType == WeaponType.SWORD && attackPos != null)
        {
            attackPos.localPosition = mouseDirection * attackRange;
        }

        // Attacking
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            switch (weaponType)
            {
                case WeaponType.GUN:
                    if (pistolAmmo > 0 && timeSinceLastShot <= 0 && reloadTimeRemaining <= 0)
                    {
                        ShootGun(mouseDirection);
                        pistolAmmo--; // Decrease ammo count
                        timeSinceLastShot = pistolCooldown; // Set cooldown after shooting
                    }
                    else if (reloadTimeRemaining <= 0 && timeSinceLastShot <= 0)
                    {
                        AutoReload(WeaponType.GUN); // Automatically reload the gun if out of ammo
                    }
                    break;

                case WeaponType.SWORD:
                    SwordAttack();
                    break;

                case WeaponType.SHOTGUN:
                    if (shotgunAmmo > 0 && timeSinceLastShot <= 0 && reloadTimeRemaining <= 0)
                    {
                        ShootShotgun(mouseDirection);
                        shotgunAmmo--; // Decrease ammo count
                        timeSinceLastShot = shotgunCooldown; // Set cooldown after shooting
                    }
                    else if (reloadTimeRemaining <= 0 && timeSinceLastShot <= 0)
                    {
                        AutoReload(WeaponType.SHOTGUN); // Automatically reload the shotgun if out of ammo
                    }
                    break;
            }
        }

        // Handle cooldowns, reloading, and automatic reloading logic
        if (timeSinceLastShot > 0)
        {
            timeSinceLastShot -= Time.deltaTime; // Decrease cooldown time
        }

        if (reloadTimeRemaining > 0)
        {
            reloadTimeRemaining -= Time.deltaTime; // Decrease reload time
        }

        if (weaponType == WeaponType.SWORD && timeBtwAttack > 0)
        {
            timeBtwAttack -= Time.deltaTime;
        }

        // Cycle Weapon

        if (Input.GetKeyDown(KeyCode.R))
        {
            int weaponNumber = (int)++weaponType;
            weaponNumber %= (int)WeaponType.COUNT;
            weaponType = (WeaponType)weaponNumber;
            Debug.Log("Selected weapon: " + weaponType);
            switch (weaponType)
            {
                case WeaponType.GUN:
                    ani.SetBool("floatingGun", true);
                    GunSprite.SetActive(true);
                    break;

                case WeaponType.SWORD:
                    ani.SetBool("floatingGun", false);
                    GunSprite.SetActive(false);
                    break;

                case WeaponType.SHOTGUN:
                    ani.SetBool("floatingGun", true);
                    GunSprite.SetActive(true);
                    break;
            }
        }

    }

    void ShootGun(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletsPrefab);
        bullet.transform.position = transform.position + direction * 0.75f;
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;
        bullet.GetComponent<SpriteRenderer>().color = Color.white;
        Destroy(bullet, 1.0f);
    }

    void SwordAttack()
    {
        if (timeBtwAttack <= 0)
        {
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
            foreach (var enemy in enemiesToDamage)
            {
                enemy.GetComponent<Enemy>()?.TakeDamage(damage);
            }

            timeBtwAttack = startTimeBtwAttack;
        }
    }

    void ShootShotgun(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletsPrefab);
        GameObject bulletLeft = Instantiate(bulletsPrefab);
        GameObject bulletRight = Instantiate(bulletsPrefab);

        Vector3 directionLeft = Quaternion.Euler(0.0f, 0.0f, 10.0f) * direction;
        Vector3 directionRight = Quaternion.Euler(0.0f, 0.0f, -10.0f) * direction;

        bullet.transform.position = transform.position + direction * 0.75f;
        bulletLeft.transform.position = transform.position + directionLeft * 0.75f;
        bulletRight.transform.position = transform.position + directionRight * 0.75f;

        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;
        bulletLeft.GetComponent<Rigidbody2D>().linearVelocity = directionLeft * bulletSpeed;
        bulletRight.GetComponent<Rigidbody2D>().linearVelocity = directionRight * bulletSpeed;

        bullet.GetComponent<SpriteRenderer>().color = Color.white;
        bulletLeft.GetComponent<SpriteRenderer>().color = Color.white;
        bulletRight.GetComponent<SpriteRenderer>().color = Color.white;

        Destroy(bullet, 1.0f);
        Destroy(bulletLeft, 1.0f);
        Destroy(bulletRight, 1.0f);
    }

    private void AutoReload(WeaponType weapon)
    {
        if (reloadTimeRemaining <= 0)
        {
            if (weapon == WeaponType.GUN)
            {
                reloadTimeRemaining = reloadCooldownPistol; // Set reload time for pistol
                pistolAmmo = 12; // Reload the gun
                Debug.Log("Pistol Reloaded!");
            }
            else if (weapon == WeaponType.SHOTGUN)
            {
                reloadTimeRemaining = reloadCooldownShotgun; // Set reload time for shotgun
                shotgunAmmo = 6; // Reload the shotgun
                Debug.Log("Shotgun Reloaded!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (weaponType == WeaponType.SWORD && attackPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos.position, attackRange);
        }
    }
}