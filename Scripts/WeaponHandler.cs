using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Inputs")]
    public static KeyCode gunKey = KeyCode.Alpha1;

    private PickUpWeapon pickUpScript;
    private KnifeAnimations knifeScript;

    [HideInInspector] public static bool isGunActive;
    [HideInInspector] public static bool isKnifeActive;

    private void Start()
    {
        // script references
        pickUpScript = GetComponentInChildren<PickUpWeapon>();
        knifeScript = GetComponentInChildren<KnifeAnimations>();

        // default settings
        isGunActive = false;
        isKnifeActive = false;
    }

    private void Update()
    {
        // disables the knife if a gun is picked up or if the gun key is pressed
        if (pickUpScript.isWeaponPickedUp || Input.GetKeyDown(gunKey) && Shoot.currentWeapon != null && !isGunActive)
        {
            // disables knife
            EnableObject(false, KnifeAnimations.knifeObj);

            // enables gun
            EnableObject(true, Shoot.currentWeapon);

            isGunActive = true;
            isKnifeActive = false;
        }

        // disables knife even if when there is no gun
        else if (Input.GetKeyDown(gunKey) && Shoot.currentWeapon == null)
        {
            EnableObject(false, KnifeAnimations.knifeObj);
            isKnifeActive = false;
        }

        // disables gun if knife is pulled out and the gun isnt shooting
        if (Input.GetKeyDown(knifeScript.knifeKey) && !isKnifeActive && !GunAnimation.isShooting)
        {
            // disables gun
            EnableObject(false, Shoot.currentWeapon);

            // enables knife
            EnableObject(true, KnifeAnimations.knifeObj);

            // plays pullout sound
            knifeScript.PlayKnifePullOutSound();

            isGunActive = false;
            isKnifeActive = true;
        }

        // checks if the gun is active or not
        isGunActive = Shoot.currentWeapon != null && Shoot.currentWeapon.activeInHierarchy;
    }

    private void EnableObject(bool active, GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(active);
        }
    }
}
