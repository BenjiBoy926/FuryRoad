using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[System.Obsolete("DriftingAudio is obsolete. Audio should be played " +
    "directly on an audio source and synchronized over the network")]
public class DriftingAudio
{
    [SerializeField]
    [Tooltip("Audio source that plays the dirt skid sound")]
    private AudioSource skidSource;
    [SerializeField]
    [Tooltip("List of audio clips randomly chosen for the dirt slide effect")]
    private List<AudioClip> skidSounds;

    // Play a random skid sound from the list of skid sounds
    public void PlaySkidSound()
    {
        int sel = Random.Range(0, skidSounds.Count);
        skidSource.clip = skidSounds[sel];
        skidSource.Play();
    }
}
