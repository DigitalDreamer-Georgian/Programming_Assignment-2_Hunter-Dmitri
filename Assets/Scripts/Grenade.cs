using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField]
    GameObject shrapnelPrefab;

    void Start()
    {
        Invoke(nameof(SelfDestruct), 0.05f);
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        int shrapnelCount = 6;
        float angle = 360.0f / shrapnelCount;
        for (int i = 0; i < shrapnelCount; i++)
        {
            Vector3 direction = Quaternion.Euler(0.0f, 0.0f, angle * i) * Vector3.right;
            GameObject shrapnel = Instantiate(shrapnelPrefab);
            shrapnel.transform.position = transform.position + direction * 0.75f;
            shrapnel.GetComponent<Rigidbody2D>().linearVelocity = direction * 2.5f;
            shrapnel.GetComponent<SpriteRenderer>().color = Color.magenta;
            Destroy(shrapnel, 1.0f);
        }
    }
}
