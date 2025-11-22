using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Settings")]
    public float health;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    public void DecreaseHealth()
    {
        health -= Shoot.damage;

        if (health <= 0.0f)
        {
            rb.constraints = RigidbodyConstraints.None;
            Destroy(gameObject);
        }
    }
}
