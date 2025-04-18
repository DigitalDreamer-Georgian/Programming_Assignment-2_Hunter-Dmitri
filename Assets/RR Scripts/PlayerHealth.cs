
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    int spawn = 0;
    private float health = 0f;
    public string nextlvl;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Transform respawn;
    [SerializeField] private HealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    private void Start()
    {
        health = maxHealth;
        healthBar?.UpdateHealthBar(health, maxHealth);
    }

    public void UpdateHealth(float mod)
    {
        health += mod;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health <= 0f)
        {
            health = 0f;
            Respawn();
        }

        healthBar?.UpdateHealthBar(health, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0;
        }
        healthBar?.UpdateHealthBar(health, maxHealth);

        if (health <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        //this code if anyone whants to touch is for sending player to game over
        
        //Debug.Log("Deaths "+ spawn);
       // if (spawn == 3)
       // {
      //      SceneManager.LoadScene(5);
       // }


        health = maxHealth;
        healthBar?.UpdateHealthBar(health, maxHealth);
        if (respawn != null)
        {
            //transform.position = respawn.position;
            SceneManager.LoadScene(nextlvl);
    
        
            //This sends player to gameover
           // SceneManager.LoadScene(5);
        }
    }
}
