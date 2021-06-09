using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoostAudio
{
    [SerializeField]
    [Tooltip("Audio source that plays audio during the full boost")]
    private AudioSource audio;
    [SerializeField]
    [Tooltip("One shot clip that plays at the start of the boost")]
    private AudioClip clip;

    [SerializeField]
    [Tooltip("Min-max pitch range for the audio clip")]
    private FloatRange pitchRange = new FloatRange(1f, 1.5f);
    [SerializeField]
    [Tooltip("Minumum volume level of the underscore audio")]
    private float minUnderscoreVolume = 0.1f;
    [SerializeField]
    [Tooltip("Time it takes for the audio to fade out when finished")]
    private float fadeTime = 1f;

    public void StartAudio()
    {
        // Play the audio that underscores the boost
        audio.clip = clip;
        audio.Play();
    }

    public void FixedUpdate(float boostLevel)
    {
        // Change volume of the underscore as the boost increases/decreases level
        //audio.volume = Mathf.Lerp(minUnderscoreVolume, 1f, boostLevel);
        // Change pitch based on current boost level
        audio.pitch = Mathf.LerpUnclamped(pitchRange.min, pitchRange.max, boostLevel);
    }

    public void StopAudio()
    {
        // We need this to fade out tho, but we can't do it since this is not a MonoBehaviour...
        // Time to check out the "DOTween" package!
        audio.Stop();
    }
}
