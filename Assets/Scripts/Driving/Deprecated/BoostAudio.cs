using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

[System.Serializable]
[System.Obsolete("Boost audio is obsolete, use BoostEffects instead")]
public class BoostAudio
{
    #region Private Editor Fields
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
    [Tooltip("Time it takes for the audio to fade in when started")]
    private float fadeIn = 0.2f;
    [SerializeField]
    [Tooltip("Time it takes for the audio to fade out when finsihed")]
    private float fadeOut = 1f;
    #endregion

    #region Public Methods
    public void StartAudio()
    {
        // Complete any tweens (in case the fade-out is still running)
        audio.DOComplete(true);

        // Fade the audio in
        audio.volume = 0f;
        audio.DOFade(1f, fadeIn);

        // Play the audio that underscores the boost
        audio.clip = clip;
        audio.Play();
    }
    public void UpdatePitch(float boostLevel)
    {
        // Change pitch based on current boost level
        audio.pitch = Mathf.LerpUnclamped(pitchRange.min, pitchRange.max, boostLevel);
    }
    public void StopAudio()
    {
        // Complete any tweens (in case the fade-in is still running)
        audio.DOComplete(true);

        // Fade the audio out
        audio.volume = 1f;
        audio.DOFade(0f, fadeOut).OnComplete(() => audio.Stop());
    }
    #endregion
}
