using UnityEngine;

public class DropWeapon : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode dropKey = KeyCode.G;

    [Header("Settings")]
    public float forwardForce = 10.0f;
    public float upwardForce = 5.0f;

    private Rigidbody rb;
    private Transform weaponHolder;
    private GameObject currentWeapon;

    private PickUpWeapon pickupScript;

    private Camera cam;

    [HideInInspector] public bool isWeaponHolderEmpty;
    private void Start()
    {
        weaponHolder = GameObject.Find("WeaponHolder").transform;

        cam = GameObject.Find("PlayerCam").GetComponent<Camera>();

        // script references
        pickupScript = GetComponent<PickUpWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        isWeaponHolderEmpty = weaponHolder.childCount <= 0;

        // if the weaponholder isnt empty and the gun isnt shooting to fix audio bugs
        if (!isWeaponHolderEmpty && !GunAnimation.isShooting) {
            currentWeapon = weaponHolder.GetChild(0).gameObject;

            // drops the gun
            if (Input.GetKeyDown(dropKey) && WeaponHandler.isGunActive) {

                // saves the amount off bullet left in the gun
                StoreCurrentBullet(currentWeapon.transform);

                // bug fixing
                if (pickupScript.isCoroutineActive)
                {
                    StopCoroutine(pickupScript.pickupCoroutine);
                } 

                // removes its parents
                currentWeapon.transform.parent = null;

                currentWeapon.GetComponent<BoxCollider>().enabled = true;

                rb = currentWeapon.AddComponent<Rigidbody>();
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

                // throwing
                rb.AddForce(cam.transform.forward * forwardForce, ForceMode.Impulse);
                rb.AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);

                // adding spin
                float randomRotation = Random.Range(-10.0f, 10.0f);
                rb.AddTorque(new Vector3(randomRotation, randomRotation, randomRotation));
            }
        }
    }

    private void StoreCurrentBullet(Transform weapon)
    {
        var gunInfo = weapon.transform.GetComponent<GunInfo>();

        gunInfo.StoreCurrentBullet();
    }
}
