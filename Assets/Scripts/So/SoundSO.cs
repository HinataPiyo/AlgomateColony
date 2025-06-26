using UnityEngine;

[CreateAssetMenu(fileName = "SoundSO", menuName = "GameSystem/SoundSO")]
public class SoundSO : ScriptableObject
{
    [Header("BGM")]
    public AudioClip[] bgms;

    [Header("SE")]
    public AudioClip[] se_system;
}
