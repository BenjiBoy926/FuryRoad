using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingAudio : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Audio source that plays the engine audio")]
    private AudioSource engineAudioSource;
    [SerializeField]
    [Tooltip("Audio clip that plays while the car is idle")]
    private AudioClip engineIdleAudio;
    [SerializeField]
    [Tooltip("Audio clip that plays while the car is driving")]
    private AudioClip engineDrivingAudio;
    [SerializeField]
    [Tooltip("Min-max pitch for the driving audio")]
    private FloatRange engineAudioPitchRange;
    #endregion

    #region Private Fields
    // Top speed of the racer
    private float topSpeed;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();
        topSpeed = m_Manager.topSpeedModule.baseTopSpeed;
    }
    private void FixedUpdate()
    {
        float absSpeed = Mathf.Abs(m_Manager.forwardSpeed);

        if (absSpeed < 5f)
        {
            // Check to swap the clips
            if (engineAudioSource.clip != engineIdleAudio)
            {
                engineAudioSource.clip = engineIdleAudio;
                engineAudioSource.Play();
            }

            // Pitch is min when idle
            engineAudioSource.pitch = engineAudioPitchRange.min;
        }
        else
        {
            // Check to swap the clips
            if (engineAudioSource.clip != engineDrivingAudio)
            {
                engineAudioSource.clip = engineDrivingAudio;
                engineAudioSource.Play();
            }

            // Lerp the pitch of the audio source so that higher pitch as it goes faster
            float interpolator = absSpeed / topSpeed;
            engineAudioSource.pitch = Mathf.LerpUnclamped(engineAudioPitchRange.min, engineAudioPitchRange.max, interpolator);
        }
    }
    #endregion
}
