
using UnityEngine;
using UnityEngine.UI;

public class GunUIManager : MonoBehaviour
{
    [Header("References")]
    public Image crosshair;
    public Image reloadUI;

    private Shoot shootScript;
    private DropWeapon dropWeaponScript;

    private float time;
    private float desiredFillAmount;

    private void Start()
    {
        // script references
        shootScript = GetComponent<Shoot>();
        dropWeaponScript = GetComponent<DropWeapon>();

        // disables the reloadUI at start
        reloadUI.enabled = false;

        // sets the value of reloadUI to 0
        reloadUI.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // updates the UI fill amount
        if (GunAnimation.isReloading)
        {
            reloadUI.enabled = true;
            crosshair.enabled = false;

            time += Time.deltaTime;
            desiredFillAmount = Mathf.Clamp01(time / Shoot.reloadTime);
            reloadUI.fillAmount = desiredFillAmount;
        }

        // resets after dropping the gun while reloading
        if (dropWeaponScript.isWeaponHolderEmpty || !WeaponHandler.isGunActive)
        {
            GunAnimation.isReloading = false;
            ResetReloadUI();
        }

        // resets after reaching the max
        if (desiredFillAmount == 1.0f)
        {
            Invoke(nameof(ResetReloadUI), 0.5f);
        }
    }
    private void ResetReloadUI()
    {
        // reenables crosshair and disables reloadUI
        crosshair.enabled = true; 
        reloadUI.enabled = false;

        // resets
        time = 0.0f;
        desiredFillAmount = 0.0f;
        reloadUI.fillAmount = 0.0f;
    }
}