using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DriftingModule
{
    [SerializeField]
    [Tooltip("Time that the module must be drifting for in order to receive a drift boost at the end")]
    private float driftBoostChargeTime;
    [SerializeField]
    [Tooltip("Amount that the steering changes while the drift is active.  " +
        "A value of 1 indicates that the vehicle moves straight forward while turning against the drift," +
        "a value of 0 indicates that the vehicle's turning will not be modified at all")]
    private float steeringModifier;
    [SerializeField]
    [Tooltip("If the speed of the rigidbody falls below this threshold, the drifting module cancels the drift without boosting")]
    private float cancelThreshold = 5f;
    [SerializeField]
    [Tooltip("Boost that the rigidbody receives when the drift is finished")]
    private BoostingModule m_DriftBoost;

    // Direction that the vehicle is drifting, either -1 for left or 1 for right
    private float m_CurrentDirection;
    // True if the drifting module is actively drifting
    private bool m_DriftActive;
    // Time when the drifting started
    private float m_DriftStartTime;

    public bool driftActive => m_DriftActive;
    public float currentDirection => m_CurrentDirection;
    public BoostingModule driftBoost => m_DriftBoost;

    public void Start()
    {
        m_DriftBoost.Start();
    }

    public void FixedUpdate(Vector3 velocity, Vector3 heading)
    {
        // If the velocity magnitude falls below the threshold, cancel the drift
        if(velocity.sqrMagnitude <= (cancelThreshold * cancelThreshold))
        {
            StopDrifting();
        }
        // Always update the drift boost
        m_DriftBoost.FixedUpdate(heading);
    }

    // Get the steering of the drifting module.
    // If we are not drifting, let the steering pass through unchanged
    // If we are drifting, remap the steering from (-1, 1) -> (-2, 0) for left drift
    // and (-1, 1) -> (0, 2) for right drift
    public float GetSteer(float steer)
    {
        if (m_DriftActive)
        {
            return steer + (m_CurrentDirection * steeringModifier);
        }
        else return steer;
    }

    public bool TryStartDrifting(GroundingModule groundingModule, float h)
    {
        if(!m_DriftActive && (h < -0.001 || h > 0.001) && groundingModule.grounded)
        {
            StartDrifting(h);
            return true;
        }
        return false;
    }

    public void StartDrifting(float h)
    {
        // Set drifting to be active
        m_DriftActive = true;
        m_DriftStartTime = Time.time;

        // Set drifting direction
        if (h < 0)
        {
            m_CurrentDirection = -1f;
        }
        else m_CurrentDirection = 1f;
    }

    // Finish the drift by checking if we have drifted enough for a boost
    public void FinishDrifting(Rigidbody rb, float topSpeed, Vector3 heading)
    {
        // If we have been drifting long enough to charge the drift boost, then boost!
        if (m_DriftActive && Time.time - m_DriftStartTime > driftBoostChargeTime)
        {
            m_DriftBoost.StartBoosting(rb, topSpeed, heading);
        }
        StopDrifting();
    }

    // Stop the drift, without trying to check the boost
    public void StopDrifting()
    {
        m_DriftActive = false;
    }
}
