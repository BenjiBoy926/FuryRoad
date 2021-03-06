﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoostingModule : ITopSpeedModifier
{
    [SerializeField]
    [Tooltip("Modification of the top speed while boost is at the peak speed")]
    private float m_MaxTopSpeedModifier = 1.5f;
    [SerializeField]
    [Tooltip("Duration of the boost")]
    private float m_BoostDuration = 2f;
    [SerializeField]
    [Tooltip("Curve used to determine the boosting speed of the car.")]
    private AnimationCurve m_BoostCurve;
    [SerializeField]
    [Tooltip("References to the particle systems that activate during the boost")]
    private List<ParticleSystem> m_JetstreamParticles;

    [Header("Audio")]

    [SerializeField]
    [Tooltip("Manages the audio for the boost")]
    private BoostAudio audio;

    [Header("Events")]

    [SerializeField]
    [Tooltip("Event invoked when the boost begins")]
    private UnityEvent m_OnBoostBegin;
    [SerializeField]
    [Tooltip("Event invoked each time the boost module updates")]
    private FloatEvent m_OnBoostUpdate;
    [SerializeField]
    [Tooltip("Event invoked when the boost ends")]
    private UnityEvent m_OnBoostEnd;

    // The time when the boost began
    private float m_BoostBeginTime = 0f;
    // True if the boost has stopped
    private bool m_BoostHasStopped = true;

    // Public getters for the events
    public UnityEvent onBoostBegin => m_OnBoostBegin;
    public UnityEvent<float> onBoostUpdate => m_OnBoostUpdate;
    public UnityEvent onBoostEnd => m_OnBoostEnd;

    // True if the boost is still active
    public bool boostActive => Time.time < m_BoostBeginTime + m_BoostDuration;
    // Amount of time that the module has been boosting
    public float currentBoostTime => Time.time - m_BoostBeginTime;
    public float currentBoostInterpolator => currentBoostTime / m_BoostDuration;
    // Current modification of the vehicle's top speed, 
    // changes based on the current time in the boost
    public float topSpeedModifier => Mathf.LerpUnclamped(1f, m_MaxTopSpeedModifier, m_BoostCurve.Evaluate(currentBoostInterpolator));

    // Implement the interface "ITopSpeedModifier"
    public float modifier => topSpeedModifier;
    public bool applyModifier => boostActive;

    public void Awake()
    {
        // Set so that we do not think we are boosting at the start of the game
        m_BoostBeginTime = -m_BoostDuration - 1f;

        // Add audio updates to the events
        m_OnBoostBegin.AddListener(audio.StartAudio);
        m_OnBoostUpdate.AddListener(audio.FixedUpdate);
        m_OnBoostEnd.AddListener(audio.StopAudio);
    }

    public void FixedUpdate(Rigidbody rb, float topSpeed, Vector3 heading, Vector3 normal)
    {
        // If the boost is in progress, set the velocity to boost speed
        if (boostActive)
        {
            // Separate the speed into component perpendicular to motion
            // and speed in the plane of motion (planes change based on normal vector)
            rb.velocity = heading * topSpeed + Vector3.Project(rb.velocity, normal);
            m_OnBoostUpdate.Invoke(m_BoostCurve.Evaluate(currentBoostInterpolator));
        }
        // If the boost is inactive but the boost has not been stopped, then stop the boost
        // This is used so that we can invoke an event on the first frame that the boost stops
        else if(!m_BoostHasStopped)
        {
            StopBoosting();
        }
    }

    public bool TryStartBoosting(GroundingModule groundingModule, Rigidbody rb, float startSpeed, Vector3 heading)
    {
        if(!boostActive && groundingModule.grounded)
        {
            StartBoosting(rb, startSpeed, heading);
            return true;
        }
        return false;
    }

    public void StartBoosting(Rigidbody rb, float startSpeed, Vector3 heading)
    {
        // At the start of the boost, set the velocity to the top speed
        rb.velocity = heading * startSpeed;

        // Set the time when the boost began
        m_BoostBeginTime = Time.time;

        // Invoke the boost begin event and play the particles
        onBoostBegin.Invoke();
        m_BoostHasStopped = false;
        SetParticlesActive(true);
    }

    public void StopBoosting()
    {
        // Invoke the boost end event and stop the particles
        onBoostEnd.Invoke();
        m_BoostHasStopped = true;
        SetParticlesActive(false);
    }

    private void SetParticlesActive(bool active)
    {
        foreach(ParticleSystem system in m_JetstreamParticles)
        {
            if (active) system.Play();
            else system.Stop();
        }
    }

    [System.Serializable]
    private class FloatEvent : UnityEvent<float> { }
}
