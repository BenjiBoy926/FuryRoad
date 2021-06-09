using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrivingAudio
{
    [SerializeField]
    [Tooltip("Audio source that plays the engine audio")]
    private AudioSource audio;
    [SerializeField]
    [Tooltip("Audio clip that plays while the car is idle")]
    private AudioClip idleAudio;
    [SerializeField]
    [Tooltip("Audio clip that plays while the car is driving")]
    private AudioClip drivingAudio;
    [SerializeField]
    [Tooltip("Min-max pitch for the driving audio")]
    private FloatRange pitchRange;

    // Top speed of the racer
    private float topSpeed;

    public void Start(float topSpeed)
    {
        this.topSpeed = topSpeed;
    }

    public void FixedUpdate(float speed)
    {
        if(speed < 5f)
        {
            // Check to swap the clips
            if(audio.clip != idleAudio)
            {
                audio.clip = idleAudio;
                audio.Play();
            }

            // Pitch is min when idle
            audio.pitch = pitchRange.min;
        }
        else
        {
            // Check to swap the clips
            if(audio.clip != drivingAudio)
            {
                audio.clip = drivingAudio;
                audio.Play();
            }

            // Lerp the pitch of the audio source so that higher pitch as it goes faster
            float interpolator = speed / topSpeed;
            audio.pitch = Mathf.LerpUnclamped(pitchRange.min, pitchRange.max, interpolator);
        }
    }
}
