using UnityEngine;
using EZCameraShake;

public class CameraShake : MonoBehaviour
{
    public static void KnifeShake()
    {
        // checks to see if the player is on the wall
        // shaking the camera while being tilted is a bad idea
        if (CameraShaker.Instance.isActiveAndEnabled)
            CameraShaker.Instance.ShakeOnce(4f, 1.5f, .1f, .5f);
    }

    public static void ShootingShake()
    {
        CameraShaker.Instance.ShakeOnce(5f, 1f, .1f, .5f);
    }
}
