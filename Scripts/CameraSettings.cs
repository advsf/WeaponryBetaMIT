using UnityEngine;

public class CameraSettings : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.instance.wallrunning)
        {
            EZCameraShake.CameraShaker.Instance.enabled = false;
        }

        // reenables camerashaker if isnt wallrunning and isnt wall jumping (on air)
        if (!PlayerMovement.instance.wallrunning && PlayerMovement.instance.isGrounded)
        {
            EZCameraShake.CameraShaker.Instance.enabled = true;
        }
    }
}
