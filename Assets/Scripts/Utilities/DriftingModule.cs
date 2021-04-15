using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DriftingModule
{
    [SerializeField]
    [Tooltip("Min-max radii for the vehicle's drifting")]
    private FloatRange driftAngleRange;
    [SerializeField]
    [Tooltip("Time that the module must be drifting for in order to receive a drift boost at the end")]
    private float driftBoostChargeTime;
    [SerializeField]
    [Tooltip("Boost that the rigidbody receives when the drift is finished")]
    private BoostingModule driftBoost;

    // Rigidbody with affected velocity for the drift
    private Rigidbody m_Rigidbody;
    // Direction that the vehicle is drifting
    private Vector3 m_Dir;
    // True if the drifting module is actively drifting
    private bool m_DriftActive;
    // Time when the drifting started
    private float m_DriftStartTime;

    public void Update(float h)
    {
        if(m_DriftActive)
        {
            // MEH?!
        }

        // Always update the drift boost
        driftBoost.Update();
    }

    public bool TryStartDrifting(GroundingModule groundingModule, Rigidbody rigidbody, float h)
    {
        if(!m_DriftActive && (h < -0.001 || h > 0.001) && groundingModule.Grounded())
        {
            StartDrifting(rigidbody, h);
            return true;
        }
        return false;
    }

    public void StartDrifting(Rigidbody rigidbody, float h)
    {
        // Set drifting to be active
        m_DriftActive = true;
        m_DriftStartTime = Time.time;
        m_Rigidbody = rigidbody;

        // Set drifting direction
        if (h < 0)
        {
            m_Dir = Vector3.left;
        }
        else m_Dir = Vector3.right;
    }

    public void StopDrifting(float topSpeed)
    {
        // If we have been drifting long enough to charge the drift boost, then boost!
        if (m_DriftActive && Time.time - m_DriftStartTime > driftBoostChargeTime)
        {
            driftBoost.StartBoosting(m_Rigidbody, topSpeed);
        }

        m_DriftActive = false;
    }
}
