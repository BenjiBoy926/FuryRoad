using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoostingModule
{
    [SerializeField]
    [Tooltip("Max speed of the boost, added on top of top speed")]
    private float m_BoostSpeed = 90f;
    [SerializeField]
    [Tooltip("Duration of the boost")]
    private float m_BoostDuration = 2f;
    [SerializeField]
    [Tooltip("Curve used to determine the boosting speed of the car.")]
    private AnimationCurve m_BoostCurve;
    [SerializeField]
    [Tooltip("Reference to the game object that displays the jetstream particle when the player boosts")]
    private GameObject m_JetstreamParticle;

    public float boostDuration => m_BoostDuration;

    // The time when the boost began
    private float m_BoostBeginTime;
    public bool boostActive => boostInterpolator < 1f;
    public float boostTime => Time.time - m_BoostBeginTime;
    public float boostInterpolator => boostTime / m_BoostDuration;
    
    public float BoostSpeed(float topSpeed)
    {
        return topSpeed + (m_BoostCurve.Evaluate(boostInterpolator) * m_BoostSpeed);
    }

    public bool TryBeginBoosting()
    {
        if(!boostActive)
        {
            BeginBoosting();
            return true;
        }
        return false;
    }

    public void BeginBoosting()
    {
        m_BoostBeginTime = Time.time;
        SetEffectsActive(true);
    }

    public void SetEffectsActive(bool active)
    {
        m_JetstreamParticle.SetActive(active);
    }
}
