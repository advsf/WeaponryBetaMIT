using UnityEngine;
using System;

public class KnifeAnimations : MonoBehaviour
{
    [Header("Inputs")]
    public KeyCode knifeKey = KeyCode.Alpha3;
    public KeyCode meleeKey1 = KeyCode.Mouse0;
    public KeyCode meleeKey2 = KeyCode.Mouse1;

    // animation
    [Header("References")]
    public Animator karambitAnimator;
    public ParticleSystem trail;
    private Animator currentKnife;

    // audio
    [Header("Karambit")]
    public AudioClip karambitPullOutSound;
    public AudioClip karambitSwingSound;

    private AudioSource source;
    private AudioClip pullOutSound;

    private int Melee1Hash;
    private int Melee2Hash;

    // misc
    [HideInInspector] public static GameObject knifeObj;
    [HideInInspector] public static bool isMeleeing;

    private void Start()
    {
        // hash bools for performance
        Melee1Hash = Animator.StringToHash("isMelee1");
        Melee2Hash = Animator.StringToHash("isMelee2");

        // disables knife in the beginning
        if (transform.GetChild(0) != null)
            transform.GetChild(0).gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        // gathers info about the knife
        if (transform.childCount > 0)
        {
            GetCurrentKnife();
        }

        // melee 1 animation (left mouse)
        if (Input.GetKey(meleeKey1))
        {
            PlayMelee1Animation();
            isMeleeing = true;
        }

        if (Input.GetKeyUp(meleeKey1))
        {
            StopPlayMelee1Animation();
            isMeleeing = false;
        }

        // melee 2 animation (right mouse)
        if (Input.GetKey(meleeKey2))
        {
            PlayMelee2Animation();
            isMeleeing = true;
        }

        if (Input.GetKeyUp(meleeKey2))
        {
            StopPlayMelee2Animation();
            isMeleeing = false;
        }

        // particlesystem
        if (Input.GetKey(meleeKey1) || Input.GetKey(meleeKey2))
        {
            trail.Emit(1);
        }
    }

    private void GetCurrentKnife()
    {
        // <summary>
        // sets the currentknife's animation to be set to the current equipped knife
        // <summary>

        // sets the gameobject of the knife
        knifeObj = transform.GetChild(0).gameObject;

        // sets the animation and audio clips to be the karambits
        if (knifeObj.name == "Karambit")
        {
            // animation
            currentKnife = karambitAnimator;

            // audio clip
            pullOutSound = karambitPullOutSound;
        } 

        // gets the audio source
        source = knifeObj.GetComponent<AudioSource>();
    }

    private void PlayMelee1Animation()
    {
        currentKnife.SetBool(Melee1Hash, true);
    }

    private void StopPlayMelee1Animation()
    {
        currentKnife.SetBool(Melee1Hash, false);
    }

    private void PlayMelee2Animation()
    {
        currentKnife.SetBool(Melee2Hash, true);
    }

    private void StopPlayMelee2Animation()
    {
        currentKnife.SetBool(Melee2Hash, false);
    }

    public void PlayKarambitSlashSound()
    {
        source.PlayOneShot(karambitSwingSound);
    }

    public void PlayKnifePullOutSound()
    {
        source.PlayOneShot(pullOutSound);
    }
}
