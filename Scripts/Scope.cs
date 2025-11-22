using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Scope : MonoBehaviour
{
    [Header("References")]
    public KeyCode zoomInKey = KeyCode.Mouse1;
    public Camera playerCam;
    public Transform orientation;
    public Image scopeImage;
    public Image crosshair;

    [Header("Settings")]
    public float zoomTime;
    private float originalScopeAmount;

    [HideInInspector] public bool isZoomed;

    private DropWeapon dropWeaponScript;
    private void Start()
    {
        // stores the original
        originalScopeAmount = playerCam.fieldOfView; 

        // default setting
        isZoomed = false;
        scopeImage.enabled = false;

        // script reference
        dropWeaponScript = GetComponent<DropWeapon>();
    }
    // Update is called once per frame
    void Update()
    {
        // scopes in
        if (Input.GetKeyDown(zoomInKey) && !isZoomed && !dropWeaponScript.isWeaponHolderEmpty && WeaponHandler.isGunActive && !GunAnimation.isShooting)
        {
            ScopeIn();
        }

        // scopes out
        else if ((Input.GetKeyDown(zoomInKey) && isZoomed) || dropWeaponScript.isWeaponHolderEmpty || !WeaponHandler.isGunActive || GunAnimation.isShooting) 
        {
            Invoke(nameof(ScopeOut), 0.1f);
        } 
    }

    private void ScopeIn()
    {
        isZoomed = true;

        // sets fov to stimulate scoping in
        SetCamFOV(Shoot.zoomAmount);

        // disables UI if the weapon can scope in
        if (Shoot.canScope)
        {
            scopeImage.enabled = true;
            crosshair.enabled = false;
        }
    }

    public void ScopeOut()
    {
        isZoomed = false;

        // sets fov to stimulate scoping in
        SetCamFOV(originalScopeAmount);

        // disables UI
        scopeImage.enabled = false;
        crosshair.enabled = true;
    }

    private void SetCamFOV(float amount)
    {
        playerCam.DOFieldOfView(amount, 0.25f);
    }
}
