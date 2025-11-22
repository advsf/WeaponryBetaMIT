using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    // reference to the scriptableobject
    public GunSoundData gunSoundData;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource source;

    // adjust the length parameter when adding new sounds to guns
    private AudioClip[] currentGunSoundClips = new AudioClip[3];

    [Header("References")]
    private DropWeapon dropWeaponScript;
    private Shoot Shoot;
    private Scope scopeScript;

    // animator hashes
    private int isReloadingHash;
    private int isShootingHash;
    private int isPulledOutHash;
    private int isIdleHash;

    [HideInInspector] public static bool isReloading;
    [HideInInspector] public static bool isShooting;
    private bool isPulledOut;
    private Animator currentGunAnimator;

    private void Start()
    {
        // performance boost for animations
        isReloadingHash = Animator.StringToHash("isReloading");
        isShootingHash = Animator.StringToHash("isShooting");
        isPulledOutHash = Animator.StringToHash("isPulledOut");
        isIdleHash = Animator.StringToHash("isIdle");

        // references to scripts
        dropWeaponScript = GetComponent<DropWeapon>();
        Shoot = GetComponent<Shoot>();
        scopeScript = GetComponent<Scope>();
    }

    // Update is called once per frame
    void Update()
    {
        // sets the animator and sound of each unique guns
        SetGunAnimatorAndSound();

        // reloading animation
        if (Input.GetKeyDown(Shoot.reloadKey) && Shoot.currentBullet < Shoot.magazineSize && !isReloading && !isShooting && WeaponHandler.isGunActive)
        {
            PlayReloadAnimation();
        }

        // recoil animation and sound effect
        if (Shoot.isShooting && !isShooting && WeaponHandler.isGunActive)
        {
            PlayRecoilAnimation();
        }

        // pull out animation
        if (Input.GetKeyDown(WeaponHandler.gunKey) && !isPulledOut)
        {
            isPulledOut = true;
            Invoke(nameof(PlayPullOutAnimation), 0.05f);
        }

        // pull out animation when first picking up a weapon
        else if (Input.GetKeyDown(PickUpWeapon.pickupKey) && !isPulledOut)
        {
            isPulledOut = true;
            // add delay to allow the currentweapon to be set to the weapon
            Invoke(nameof(PlayPullOutAnimation), 0.05f);
        }

        // checks if the gun is still equipped, else disable animation and sound
        if (dropWeaponScript.isWeaponHolderEmpty)
        {
            // disables reloading
            if (isReloading)
            {
                currentGunAnimator.SetBool(isReloadingHash, false);
                isReloading = false;
            }

            DisableAnimation();
            DisableSound();
        }

        // unscopes when reloading
        if (scopeScript.isZoomed && isReloading)
        {
            scopeScript.ScopeOut();
        }

        // sets isPulledOut to false when the gun is no longer active
        if (!WeaponHandler.isGunActive)
        {
            isPulledOut = false;
            currentGunAnimator = null;
        } 
    }

    private void SetGunAnimatorAndSound()
    {
        // gets the animator component and sound
        if (transform.childCount >= 1)
        {
            currentGunAnimator = transform.GetChild(0).GetComponent<Animator>();

            // sets the audioclips to the specific gun
            if (transform.GetChild(0).name.Contains("Sheriff"))
            {
                currentGunSoundClips = gunSoundData.sheriffSounds;
            }
            else if (transform.GetChild(0).name.Contains("Sniper"))
            {
                currentGunSoundClips = gunSoundData.sniperSounds;
            }
        }
    }

    private void PlayReloadAnimation()
    {
        // disables shooting
        isReloading = true;

        // reenables animation
        currentGunAnimator.enabled = true;

        currentGunAnimator.SetBool(isReloadingHash, true);

        // plays the reload sound clip (index is 1)
        AudioManager.instance.PlayOneShot(source, currentGunSoundClips[1]);

        Invoke(nameof(StopReloadAnimation), Shoot.reloadTime);
    }

    private void StopReloadAnimation()
    {
        currentGunAnimator.SetBool(isReloadingHash, false);

        // changing the currentbullet to the magazinesize
        if (isReloading)
        {
            Shoot.currentBullet = Shoot.magazineSize;
        }

        // disables animation to allow physics
        Invoke(nameof(DisableAnimation), 0.5f);
    }

    private void PlayRecoilAnimation()
    {
        // prevents any other animation from being played
        isShooting = true;

        // reenables animation
        currentGunAnimator.enabled = true;

        currentGunAnimator.SetBool(isShootingHash, true);

        // plays the recoil sound clip (index is 0)
        AudioManager.instance.PlayOneShot(source, currentGunSoundClips[0]);

        // set the timer to the animation length
        Invoke(nameof(StopRecoilAnimation), Shoot.shotCooldown);
    }

    private void StopRecoilAnimation()
    {
        currentGunAnimator.SetBool(isShootingHash, false);

        isShooting = false;

        // disables animation for physics
        Invoke(nameof(DisableAnimation), 0.01f);
    }

    private void PlayPullOutAnimation()
    {
        // reenables animation
        currentGunAnimator.enabled = true;
        currentGunAnimator.SetBool(isPulledOutHash, true);

        AudioManager.instance.PlayOneShot(source, currentGunSoundClips[2]);

        Invoke(nameof(DisablePullOutAnimation), 0.2f);
    }

    private void DisablePullOutAnimation()
    {
        currentGunAnimator.SetBool(isPulledOutHash, false);
        currentGunAnimator.SetBool(isIdleHash, true);
    }

    private void DisableAnimation()
    {
        // ensures smoother transitions
        if (isShooting)
        {
            isShooting = false;
        }

        // allows the player to shoot again
        if (isReloading)
        {
            isReloading = false;
        }

        // disables animation for physics
        if (currentGunAnimator != null)
        {
            currentGunAnimator.enabled = false;
        }
    }

    private void DisableSound()
    {
        // stops reloading sound cause it's weird if the player doesn't have the gun equipped
        AudioManager.instance.StopSound(source);
    }
}