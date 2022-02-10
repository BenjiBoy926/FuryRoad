using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;

public class BoostingEffects : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Module to check for boosting")]
    private SpeedOverTimeModule boost;
    [SerializeField]
    [Tooltip("Audio source used to play the boosting one-shot audio")]
    private AudioSource oneShotSource;
    [SerializeField]
    [Tooltip("Audio source that plays audio during the full boost")]
    private AudioSource loopSource;
    [SerializeField]
    [Tooltip("Min-max pitch range for the audio clip")]
    private FloatRange pitchRange = new FloatRange(1f, 1.5f);
    [SerializeField]
    [Tooltip("Time it takes for the audio to fade in when started")]
    private float fadeIn = 0.2f;
    [SerializeField]
    [Tooltip("Time it takes for the audio to fade out when finsihed")]
    private float fadeOut = 1f;
    [SerializeField]
    [Tooltip("List of particle effects to activate while the boost is active")]
    private ParticleSystem jetstream;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();

        // Listen for the boost events
        boost.EffectStartEvent.AddListener(OnBoostStart);
        boost.EffectUpdateEvent.AddListener(OnBoostUpdate);
        boost.EffectStopEvent.AddListener(OnBoostStop);
    }
    #endregion

    #region Event Listeners
    private void OnBoostStart()
    {
        // Play the one shot clip
        oneShotSource.Play();

        // Complete any tweens (in case the fade-out is still running)
        loopSource.DOComplete(true);

        // Fade the audio in
        loopSource.volume = 0f;
        loopSource.DOFade(1f, fadeIn);

        // Play the audio that underscores the boost
        loopSource.Play();

        // Enable the jetstream
        jetstream.Play();
    }
    private void OnBoostUpdate()
    {
        // Change pitch based on current boost interpolator
        float interpolator = boost.MagnitudeInterpolator;
        loopSource.pitch = Mathf.LerpUnclamped(pitchRange.min, pitchRange.max, interpolator);
    }
    private void OnBoostStop()
    {
        // Complete any tweens (in case the fade-in is still running)
        loopSource.DOComplete(true);

        // Fade the audio out
        loopSource.volume = 1f;
        loopSource.DOFade(0f, fadeOut).OnComplete(() => loopSource.Stop());

        // Disable the jetstream
        jetstream.Stop();
    }
    #endregion
}
