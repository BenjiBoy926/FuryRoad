using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DriftingModule : DrivingModule
{
    #region Public Properties
    public bool driftActive => m_DriftActive;
    public float currentDirection => m_CurrentDirection;
    public BoostingModule driftBoost => m_DriftBoost;
    public UnityEvent driftStartEvent => m_driftStartEvent;
    public UnityEvent driftStopEvent => m_driftStopEvent;
    public float driftDuration => Time.time - m_DriftStartTime;
    public bool driftBoostReady => m_DriftActive && driftDuration > driftBoostChargeTime;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Boost that the rigidbody receives when the drift is finished")]
    private BoostingModule m_DriftBoost;
    [SerializeField]
    [Tooltip("Amount that the heading is rotated against the drift direction when drifting begins")]
    private float m_reverseHeadingAmount = 30f;
    [SerializeField]
    [Tooltip("Time that the module must be drifting for in order to receive a drift boost at the end")]
    private float driftBoostChargeTime;
    [SerializeField]
    [Tooltip("If the speed of the rigidbody falls below this threshold, the drifting module cancels the drift without boosting")]
    private float cancelThreshold = 5f;
    [SerializeField]
    [Tooltip("Reference to the drift sparks under the left wheel")]
    private ParticleSystem leftSparks;
    [SerializeField]
    [Tooltip("Refernece to the drift sparks under the right wheel")]
    private ParticleSystem rightSparks;
    [SerializeField]
    [Tooltip("Color of the sparks before the boost is ready")]
    private ParticleSystem.MinMaxGradient boostNotReadyColor = Color.yellow;
    [SerializeField]
    [Tooltip("Color of the sparks after the boost is ready")]
    private ParticleSystem.MinMaxGradient boostIsReadyColor = Color.red;
    [SerializeField]
    [Tooltip("Handle the audio of the drift")]
    private new DriftingAudio audio;
    [SerializeField]
    [Tooltip("Event invoked when the drifting begins")]
    private UnityEvent m_driftStartEvent;
    [SerializeField]
    [Tooltip("Event invoked when the drifting stops")]
    private UnityEvent m_driftStopEvent;
    #endregion

    #region Private Fields
    // Direction that the vehicle is drifting, either -1 for left or 1 for right
    private float m_CurrentDirection;
    // True if the drifting module is actively drifting
    private bool m_DriftActive;
    // Time when the drifting started
    private float m_DriftStartTime;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();

        // Make sure sparks are inactive to begin with
        leftSparks.Stop();
        rightSparks.Stop();
    }
    private void FixedUpdate()
    {
        if (m_DriftActive)
        {
            // If the velocity magnitude falls below the threshold, and the drift is still going, then stop drifting
            if (m_Manager.rigidbody.velocity.sqrMagnitude <= (cancelThreshold * cancelThreshold))
            {
                StopDrifting();
            }
            // Set the color of the sparks when the boost is ready
            if (driftBoostReady)
            {
                SetSparkColors(boostIsReadyColor);
            }
        }
    }
    #endregion

    #region Public Methods
    // Get the steering of the drifting module.
    // If we are not drifting, let the steering pass through unchanged
    // If we are drifting, remap the steering from (-1, 1) -> (-2, 0) for left drift
    // and (-1, 1) -> (0, 2) for right drift
    public float GetSteer(float steer)
    {
        if (m_DriftActive)
        {
            // Add current direction to the steer
            return steer + m_CurrentDirection;
        }
        else return steer;
    }
    public Vector3 GetHeading()
    {
        if (m_DriftActive)
        {
            // Get the rotation angle
            float rotationAngle = m_reverseHeadingAmount * m_CurrentDirection * -1f;

            // Axis of rotation will be the ground normal of the vehicle
            Vector3 axis = manager.groundingModule.groundNormal;
            Quaternion rotation = Quaternion.AngleAxis(rotationAngle, axis);
            return rotation * manager.forward;
        }
        else return manager.forward;
    }


    public bool TryStartDrifting(float horizontalInput)
    {
        if(!m_DriftActive && 
            (horizontalInput < -0.1 || horizontalInput > 0.1) && 
            m_Manager.groundingModule.grounded && 
            m_Manager.forwardSpeed > cancelThreshold)
        {
            StartDrifting(horizontalInput);
            return true;
        }
        return false;
    }

    public void StartDrifting(float horizontalInput)
    {
        // Set drifting to be active
        m_DriftActive = true;
        m_DriftStartTime = Time.time;

        // Play a skid sound
        audio.PlaySkidSound();

        // Set drifting direction
        m_CurrentDirection = Mathf.Sign(horizontalInput);

        // Set sparks to colors for when the boost is not yet ready
        leftSparks.Play();
        rightSparks.Play();
        SetSparkColors(boostNotReadyColor);

        // Invoke the drift start event
        m_driftStartEvent.Invoke();
    }

    // Finish the drift by checking if we have drifted enough for a boost
    public void FinishDrifting()
    {
        // If we have been drifting long enough to charge the drift boost, then boost!
        if (driftBoostReady)
        {
            m_DriftBoost.StartBoosting();
        }
        StopDrifting();
    }

    // Stop the drift, without trying to check the boost
    public void StopDrifting()
    {
        if(m_DriftActive) audio.PlaySkidSound();

        // Disable the drift
        m_DriftActive = false;

        // Disable drifting sparks
        leftSparks.Stop();
        rightSparks.Stop();

        // Invoke drift stop event
        m_driftStopEvent.Invoke();
    }
    #endregion

    #region Private Methods
    private void SetSparkColors(ParticleSystem.MinMaxGradient color)
    {
        ParticleSystem.MainModule main = leftSparks.main;
        main.startColor = color;
        main = rightSparks.main;
        main.startColor = color;
    }
    #endregion
}
