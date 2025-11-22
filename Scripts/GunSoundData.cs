using UnityEngine;

[CreateAssetMenu(fileName = "GunSoundData", menuName = "ScriptableObjects/Gun Sound Data", order = 1)]
public class GunSoundData : ScriptableObject
{
    public AudioClip[] sheriffSounds;
    public AudioClip[] sniperSounds;
}