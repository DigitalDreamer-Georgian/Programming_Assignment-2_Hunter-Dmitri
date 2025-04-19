using UnityEngine;

public class BossGrenade : MonoBehaviour
{
    [SerializeField] GameObject shrapnelPrefab;
    public float lifetime = 1.5f; // Time before it explodes

    private void Start()
    {
        // Explode after the timer runs out
        Invoke(nameof(Explode), lifetime);
    }

    void Explode()
    {
        int shrapnelCount = 12;
        float angle = 360.0f / shrapnelCount;

        for (int i = 0; i < shrapnelCount; i++)
        {
            Vector3 direction = Quaternion.Euler(0f, 0f, angle * i) * Vector3.right;
            GameObject shrapnel = Instantiate(shrapnelPrefab);
            shrapnel.transform.position = transform.position + direction * 0.75f;
            shrapnel.GetComponent<Rigidbody2D>().linearVelocity = direction * 2.5f;
            shrapnel.GetComponent<SpriteRenderer>().color = Color.magenta;
            Destroy(shrapnel, 1.0f);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Optional: explode immediately on hitting something
        Explode();
    }
}
