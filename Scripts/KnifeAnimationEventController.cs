using UnityEngine;

public class KnifeAnimationEventController : MonoBehaviour
{
    private KnifeAnimations knifeScript;

    // Start is called before the first frame update
    private void Start()
    {
        knifeScript = GetComponentInParent<KnifeAnimations>();
    }

    // each method is called with animation events
    public void PlaySwingSound()
    {
        knifeScript.PlayKarambitSlashSound();
    }

    public void ShakeCamera()
    {
        // shakes the camera when meleeing
        CameraShake.KnifeShake();
    }

    public void EmitHitEffect()
    {
        if (KnifeHitDetection.instance.hit.collider != null)
        {
            KnifeHitDetection.instance.hitEffect.transform.position = KnifeHitDetection.instance.hit.point;
            KnifeHitDetection.instance.hitEffect.transform.forward = KnifeHitDetection.instance.hit.normal;
            KnifeHitDetection.instance.hitEffect.Emit(1);
        }
    }
}
