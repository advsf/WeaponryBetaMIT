using UnityEngine;

public class SheriffTracer : MonoBehaviour
{
    public float cooldown = 2.0f;

    private Shoot shootScript;

    private void Start()
    {
        shootScript = GameObject.Find("WeaponHolder").GetComponent<Shoot>();
    }

    private void Update()
    {
        // deletes clone for performance
        Invoke(nameof(DeleteBulletClone), cooldown);
    }
    private void OnCollisionEnter(Collision collision)
    {
        // sheriff bullet
        if (collision.collider != null)
        {
            Destroy(gameObject);
        }

        Health health = collision.collider.GetComponent<Health>();
        // decreases players health
        if (collision.collider.CompareTag("Enemy") && health != null)
        {
            health.DecreaseHealth();
        }
    }

    private void DeleteBulletClone()
    {
        Destroy(GameObject.Find("sheriffBulletTracer(Clone)"));
    }
}
