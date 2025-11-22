using UnityEngine;
using System.Collections;

public class PickUpWeapon : MonoBehaviour
{
    [Header("Input")]
    public static KeyCode pickupKey = KeyCode.E;

    [Header("Raycast Settings")]
    [SerializeField] private float distance = 5.0f;
    [SerializeField] private float radius = 0.75f;

    [Header("Animation Settings")]
    [SerializeField] private float pickupTime = 2f;

    private Rigidbody rb;
    private Transform cam;
    private Transform weaponHolder;

    private Quaternion originalLocalRotation;
    private Vector3 originalLocalPosition;
    private RaycastHit hit;

    private DropWeapon dropWeaponScript;

    [HideInInspector] public bool isCoroutineActive;
    [HideInInspector] public bool isWeaponPickedUp;
    [HideInInspector] public Coroutine pickupCoroutine;
    private void Start()
    {
        // refernces
        cam = GameObject.Find("PlayerCam").transform;
        weaponHolder = GameObject.Find("WeaponHolder").transform;
        
        dropWeaponScript = GetComponent<DropWeapon>();

        originalLocalRotation = Quaternion.identity;
        originalLocalPosition = new Vector3(0.0001195073f, -0.072f, -0.001153052f);
}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            // checks to see if there is a valid weapon
            Physics.SphereCast(cam.transform.position, radius, cam.forward, out hit, distance);
            if (hit.collider.CompareTag("Weapon") && dropWeaponScript.isWeaponHolderEmpty)
            {
                isWeaponPickedUp = true;

                var currentWeapon = hit.collider.transform;

                // sets the gun gameobject's parent to be the gunholder
                currentWeapon.SetParent(weaponHolder);

                // destroying rigidbody and disabling box collider
                rb = currentWeapon.GetComponent<Rigidbody>();
                Destroy(rb);

                currentWeapon.GetComponent<BoxCollider>().enabled = false;

                pickupCoroutine = StartCoroutine(PickUpSmoothly(currentWeapon));

                Invoke(nameof(SetPickUpWeaponFalse), 0.5f); 
            }
        }
    }
    private IEnumerator PickUpSmoothly(Transform currentWeapon)
    {
        // makes picking up weapons smoothly rotate and position itself to the set variables
        float elapsedTime = 0.0f;
        isCoroutineActive = true;

        while (elapsedTime < pickupTime) {
            currentWeapon.localRotation = Quaternion.Lerp(currentWeapon.localRotation, originalLocalRotation, elapsedTime / pickupTime);
            currentWeapon.localPosition = Vector3.Lerp(currentWeapon.localPosition, originalLocalPosition, elapsedTime / pickupTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentWeapon.localRotation = originalLocalRotation;
        currentWeapon.localPosition = originalLocalPosition;
        isCoroutineActive = false;
    }

    private void SetPickUpWeaponFalse()
    {
        isWeaponPickedUp = false;
    }
}
