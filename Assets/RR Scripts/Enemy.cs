using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health, maxHealth = 2;
    //public float speed;

    public HealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    private void Update()
    {
        if (health <= 0)
        {
            health = 0;
            Dead();
        }
        //transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        damage = 1;
        health -= damage;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    public virtual void Dead()
    {
        Destroy(gameObject);
    }
}
