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
    private ParticleSystem m_JetstreamParticle;

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
    // Rigidbody with affected velocity for the boost
    private Rigidbody m_Rigidbody;
    // Top speed of the car before boosting
    private float m_TopSpeed;
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
    public float boostSpeed => m_TopSpeed + (m_BoostCurve.Evaluate(currentBoostInterpolator) * m_BoostSpeed);

    public void Start()
    {
        // Set so that we do not think we are boosting at the start of the game
        m_BoostBeginTime = -m_BoostDuration - 1f;
    }

    public void FixedUpdate(Vector3 heading)
    {
        // If the boost is in progress, set the velocity to boost speed
        if (boostActive)
        {
            m_Rigidbody.AddForce(heading * boostSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
            //m_Rigidbody.velocity = heading * boostSpeed;
            m_OnBoostUpdate.Invoke(m_BoostCurve.Evaluate(currentBoostInterpolator));
        }
        // If the boost is inactive but the boost has not been stopped, then stop the boost
        // This is used so that we can invoke an event on the first frame that the boost stops
        else if(!m_BoostHasStopped)
        {
            StopBoosting();
        }
    }

    public bool TryStartBoosting(GroundingModule groundingModule, Rigidbody rb, float topSpeed, Vector3 heading)
    {
        if(!boostActive && groundingModule.grounded)
        {
            StartBoosting(rb, topSpeed, heading);
            return true;
        }
        return false;
    }

    public void StartBoosting(Rigidbody rb, float topSpeed, Vector3 heading)
    {
        // Setup the rigidbody and top speed of the car
        m_Rigidbody = rb;
        m_TopSpeed = topSpeed;

        // At the start of the boost, set the velocity to the top speed
        rb.velocity = heading * topSpeed;

        // Set the time when the boost began
        m_BoostBeginTime = Time.time;

        // Invoke the boost begin event and play the particles
        onBoostBegin.Invoke();
        m_BoostHasStopped = false;
        m_JetstreamParticle.Play();
    }

    public void StopBoosting()
    {
        // Invoke the boost end event and stop the particles
        onBoostEnd.Invoke();
        m_BoostHasStopped = true;
        m_JetstreamParticle.Stop();
    }

    [System.Serializable]
    private class FloatEvent : UnityEvent<float> { }
}
