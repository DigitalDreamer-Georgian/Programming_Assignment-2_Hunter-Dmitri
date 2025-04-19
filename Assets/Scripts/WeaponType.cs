using UnityEngine;

public enum WeaponType
{ 
    GUN,
    COUNT
}

public class Player : MonoBehaviour
{
    public Animator ani;
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    GameObject bulletsPrefab;

    float bulletSpeed = 10.0f;

    public WeaponType weaponType = WeaponType.GUN;

    // Ammo counts and reload times
    int pistolAmmo = 12;
    float pistolCooldown = 0.1f;  // Cooldown between shots for pistol
    float reloadCooldownPistol = 0.5f;  // Reload time for pistol
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

        // Attacking
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (pistolAmmo > 0 && timeSinceLastShot <= 0 && reloadTimeRemaining <= 0)
            {
                ShootGun(mouseDirection);
                pistolAmmo--;
                timeSinceLastShot = pistolCooldown;
            }
            else if (reloadTimeRemaining <= 0 && timeSinceLastShot <= 0)
            {
                AutoReload();
            }
        }

        if (timeSinceLastShot > 0)
        {
            timeSinceLastShot -= Time.deltaTime;
        }

        if (reloadTimeRemaining > 0)
        {
            reloadTimeRemaining -= Time.deltaTime;
        }

        // Cycle Weapon
        if (Input.GetKeyDown(KeyCode.R))
        {
            int weaponNumber = (int)++weaponType;
            weaponNumber %= (int)WeaponType.COUNT;
            weaponType = (WeaponType)weaponNumber;
            Debug.Log("Selected weapon: " + weaponType);

            ani.SetBool("floatingGun", true);
        }
    }

    void ShootGun(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletsPrefab);
        bullet.transform.position = transform.position + direction * 0.75f;
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;
        bullet.GetComponent<SpriteRenderer>().color = Color.blue;
        Destroy(bullet, 1.0f);
    }

    private void AutoReload()
    {
        if (reloadTimeRemaining <= 0)
        {
            reloadTimeRemaining = reloadCooldownPistol;
            pistolAmmo = 12;
            Debug.Log("Pistol Reloaded!");
        }
    }
}
