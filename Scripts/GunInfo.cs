using UnityEngine;

public class GunInfo : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float bulletSpeed = 360f;
    public float shotCooldown = 1f;
    public float reloadTime = 3f;
    public int magazineSize = 5;
    public float damage;
    public int currentBullet;
    public bool canScope;
    public float zoomAmount;
    [SerializeField] private int storedBullet;

    [Header("Bullet Tracer")]
    public GameObject tracer;

    [Header("Sound")]
    public AudioClip shootingSound;
    public AudioClip reloadingSound;
    public AudioClip pullOutSound;

    private Shoot shootScript;

    private void Start()
    {
        // sets the currentbullet to the magazinesize in the first frame
        currentBullet = magazineSize;
        storedBullet = magazineSize;

        // reference to the script
        shootScript = GameObject.Find("WeaponHolder").GetComponent<Shoot>();
    }

    // sniper bullet info
    public void GetGunInfo()
    {
        Shoot.magazineSize = magazineSize;
        Shoot.reloadTime = reloadTime;
        Shoot.shotCooldown = shotCooldown; // must be in harmony with the animation length
        Shoot.bulletSpeed = bulletSpeed;
        Shoot.damage = damage;
        Shoot.canScope = canScope; // allows weapons to scope in instead of just zooming in
        Shoot.zoomAmount = zoomAmount;

        // sets the bullet tracer to the specified weapon
        shootScript.currentBulletTracer = tracer;
    }

    public void GetCurrentBullet()
    {
        Shoot.currentBullet = storedBullet;
    }

    public void StoreCurrentBullet()
    {
        storedBullet = Shoot.currentBullet;
    }

    public void ShootShakeCamera()
    {
        // shakes the camera when shooting
        CameraShake.ShootingShake();
    }
}
