using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    #region Public Typedefs
    [System.Serializable]
    public class IntEvent : UnityEvent<int> { }
    [System.Serializable]
    public class RacingLapDataEvent : UnityEvent<RacingLapData> { }
    #endregion

    #region Public Properties
    public PlayerDriving movementDriver => m_MovementDriver;
    public Rigidbody rb => m_Rb;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the script that drives player movement")]
    private PlayerDriving m_MovementDriver;
    [SerializeField]
    [Tooltip("Reference to the rigidbody of the car")]
    private Rigidbody m_Rb;
    #endregion

    #region Public Methods
    public void EnableControl(bool active)
    {
        m_MovementDriver.enabled = active;
    }
    #endregion
}