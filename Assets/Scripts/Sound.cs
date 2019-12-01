using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    [HideInInspector] public AudioSource source;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    public bool loop;
    public bool interruptible;     // True iff this sound can interrupt itself
    [HideInInspector] public bool paused;
}