using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DriftingModule
{
    #region Public Properties
    public bool driftActive => m_DriftActive;
    public float currentDirection => m_CurrentDirection;
    public BoostingModule driftBoost => m_DriftBoost;
    #endregion

    #region Private Editor Fields
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

    [Header("Drift Boost")]

    [SerializeField]
    [Tooltip("Boost that the rigidbody receives when the drift is finished")]
    private BoostingModule m_DriftBoost;

    [Header("Drift Audio")]

    [SerializeField]
    [Tooltip("Handle the audio of the drift")]
    private DriftingAudio audio;
    #endregion

    #region Private Fields
    // Direction that the vehicle is drifting, either -1 for left or 1 for right
    private float m_CurrentDirection;
    // True if the drifting module is actively drifting
    private bool m_DriftActive;
    // Time when the drifting started
    private float m_DriftStartTime;
    #endregion

    #region Public Methods
    public void FixedUpdate(Rigidbody rb, float topSpeed, Vector3 heading, Vector3 normal)
    {
        // If the velocity magnitude falls below the threshold, cancel the drift
        if(rb.velocity.sqrMagnitude <= (cancelThreshold * cancelThreshold))
        {
            StopDrifting();
        }
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
        if(!m_DriftActive && (h < -0.1 || h > 0.1) && groundingModule.grounded)
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

        // Play a skid sound
        audio.PlaySkidSound();

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
            m_DriftBoost.StartBoosting();
        }
        StopDrifting();
    }

    // Stop the drift, without trying to check the boost
    public void StopDrifting()
    {
        if(m_DriftActive) audio.PlaySkidSound();
        m_DriftActive = false;
    }
    #endregion
}
