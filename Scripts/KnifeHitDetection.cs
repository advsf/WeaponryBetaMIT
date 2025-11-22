using UnityEngine;

public class KnifeHitDetection : MonoBehaviour
{
    public static KnifeHitDetection instance;

    [Header("Settings")]
    public float hitDistance;

    [Header("References")]
    public ParticleSystem hitEffect;
    public Camera cam;

    [HideInInspector] public RaycastHit hit;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(instance);
        }
    }

    private void Update()
    {
        // point to the center of the screen
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Physics.Raycast(ray, out hit, hitDistance);
        if (hit.collider != null && WeaponHandler.isKnifeActive)
        {

        }
    }
}
