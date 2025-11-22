using UnityEngine;
using TMPro;

public class Shoot : MonoBehaviour
{
    [Header("References")]
    public Transform weaponHolder;

    [Header("Input")]
    public KeyCode shootKey = KeyCode.Mouse0;
    public KeyCode reloadKey = KeyCode.R;

    [Header("Weapon Settings")]
    public static float bulletSpeed;
    public static float shotCooldown;
    public static float reloadTime;
    public static int magazineSize;
    public static float damage;
    public static int currentBullet;
    public static bool canScope;
    public static float zoomAmount;

    [Header("Particles")]
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;

    [Header("Bullet Tracers")]
    public GameObject sheriffBulletTracer;
    public GameObject sniperBulletTracer;
    [HideInInspector] public GameObject currentBulletTracer;

    [Header("Texts")]
    public Animator GunUI;
    public TextMeshProUGUI bulletCounterText;
    public TextMeshProUGUI magazineCounterText;
    private int isClosedHash;

    // stores the currentweapon
    [HideInInspector] public static GameObject currentWeapon;

    // communicates with the animation script to sync shooting animation
    [HideInInspector] public bool isShooting;

    private Camera cam;
    private Transform guntip;
    private Ray ray;
    private RaycastHit hitInfo;
    private Vector3 targetPoint;
    private GameObject tracer;

    // scripts
    private PickUpWeapon pickUpScript;
    private DropWeapon dropWeaponScript;
    private GunAnimation animationScript;

    private bool readyToShoot;
    
    [HideInInspector] public static bool isShot = false;

    private void Start()
    {
        // references
        cam = GameObject.Find("PlayerCam").GetComponent<Camera>();

        // animator performance boosters
        isClosedHash = Animator.StringToHash("isClosed");

        // references to the scripts
        pickUpScript = GetComponent<PickUpWeapon>();
        dropWeaponScript = GetComponent<DropWeapon>();
        animationScript = GetComponent<GunAnimation>();

        // allows the player to shoot without delay
        readyToShoot = true;

        // bug fixing (where in the first time the player shoots the muzzleflash animation wouldnt play)
        muzzleFlash.Emit(1);
    }

    // Update is called once per frame
    void Update()
    {
        // getting gun info
        if (IsGunEquipped())
        {
            // gets the current weapon
            currentWeapon = transform.GetChild(0).gameObject;

            // gets the guntip
            guntip = currentWeapon.transform.GetChild(0);

            // gets info
            GetCurrentWeaponType(currentWeapon.transform);

            // checks if the amount of bullet stored is greater than the magazine size
            // if so, set the currentbullet to magazine size
            if (currentBullet > magazineSize)
            {
                currentBullet = magazineSize;
            }
        } else
        {
            currentWeapon = null;
        }

        // current bullet status
        if (pickUpScript.isWeaponPickedUp && currentWeapon.transform != null)
        {
            GetCurrentBullet(currentWeapon.transform);
        }

        // shooting
        if (CanShoot())
        {
            ShootBullet();
        }

        // sets the bullet counter
        SetBulletCounterText();

        // limits the currentbullet to 0
        currentBullet = Mathf.Clamp(currentBullet, 0, magazineSize);
    }

    private void FixedUpdate()
    {
        // tracer physics les go
        if (isShot)
        {
            tracer.GetComponent<Rigidbody>().AddForce(targetPoint * bulletSpeed, ForceMode.Impulse);
            isShot = false;
        }
    }

    private void ShootBullet()
    {
        // syncs shooting a bullet with the animation so it doesn't look weird
        isShooting = true;

        // calculate bullet direction to the center
        ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // calculates the distance between the guntip and the center of the screen
        if (Physics.Raycast(ray, out hitInfo))
        {
            targetPoint = (hitInfo.point - guntip.position).normalized;
        } 

        // if nothing is detected
        else
        {
            targetPoint = ray.direction;
        }

        PlayGunParticle();
        currentBullet--;
        readyToShoot = false;
        Invoke(nameof(ResetShot), shotCooldown);
    }

    // should make it into a class but im too lazy
    private void GetCurrentWeaponType(Transform weapon)
    {
        GunInfo gunInfo = weapon.transform.GetComponent<GunInfo>();

        // calls the func that retrives the bullet settings
        gunInfo.GetGunInfo();

        // gets the particle system from each gun's guntip
        ParticleSystem currentMuzzleFlash = currentWeapon.transform.GetChild(0).transform.GetChild(0).GetComponent<ParticleSystem>();
        muzzleFlash = currentMuzzleFlash;
    }

    private void GetCurrentBullet(Transform weapon)
    {
        var gunInfo = weapon.transform.GetComponent<GunInfo>();

        // gets the current bullet
        gunInfo.GetCurrentBullet();
    }

    private void SetBulletCounterText()
    {
        bulletCounterText.text = currentBullet.ToString();
        magazineCounterText.text = magazineSize.ToString();

        // animations
        if (!dropWeaponScript.isWeaponHolderEmpty)
        {
            GunUI.SetBool(isClosedHash, false);
        } 
        else
        {
            GunUI.SetBool(isClosedHash, true);
        }
    }

    private void PlayGunParticle()
    {
        // bullet tracer
        tracer = Instantiate(currentBulletTracer, guntip.position, Quaternion.identity);

        // sets the bullet's layer to whatIsBullet
        // this ensures that the bullet will not collide with the shooter
        tracer.layer = LayerMask.NameToLayer("whatIsBullet");

        //allows the tracer to be shot in fixedUpdate
        isShot = true;

        // hit effect
        if (hitInfo.collider != null)
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);
        }

        // emits muzzleflash
        muzzleFlash.Emit(1);
    }
    private bool CanShoot()
    {
        return Input.GetKeyDown(shootKey) && IsGunEquipped() && readyToShoot && !GunAnimation.isReloading && currentBullet != 0 && WeaponHandler.isGunActive;
    }

    private void ResetShot()
    {
        isShooting = false;
        readyToShoot = true;
    }

    private bool IsGunEquipped()
    {
        return transform.childCount > 0;
    }
}