using UnityEngine;

public class SniperTracer : MonoBehaviour
{
    public float cooldown = 2.0f;

    private Shoot shootScript;

    private Vector3 collisionPos;
    private bool isCollided;

    private void Start()
    {
        shootScript = GameObject.Find("WeaponHolder").GetComponent<Shoot>();
    }

    private void Update()
    {
        if (isCollided)
        {
            transform.position = collisionPos;
            Invoke(nameof(DeleteBulletClone), 0f);
        } 
        else
        {
            // deletes clone for performance
            Invoke(nameof(DeleteBulletClone), cooldown);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // sniper bullet
        if (collision.collider != null)
        {
            isCollided = true;
            collisionPos = transform.position;
        }

        Health health = collision.collider.GetComponentInChildren<Health>();
        // decreases players health
        if (collision.collider.CompareTag("Enemy") && health != null)
        {
            health.DecreaseHealth();
        }
    }

    private void DeleteBulletClone()
    {
        Destroy(GameObject.Find("sniperBulletTracer(Clone)"));
    }
}
