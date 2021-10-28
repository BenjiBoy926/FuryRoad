using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDriving : MonoBehaviour
{
    #region Public Properties
    public DrivingManager drivingManager => m_DrivingManager;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the movement module that this script drives")]
    private DrivingManager m_DrivingManager;
    #endregion

    #region Private Fields
    private float m_HorizontalAxis;
    private float m_VerticalAxis;
    #endregion

    #region Monobehaviour Messages
    protected virtual void Update()
    {
        // Setup current input axes every frame
        m_HorizontalAxis = Input.GetAxis("Horizontal");
        m_VerticalAxis = Input.GetAxis("Drive");

        // Jump button fires projectiles
        if(Input.GetButtonDown("Action"))
        {
            // TODO: 
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                m_DrivingManager.projectileModule.TryFire(-1f);
            }
            else m_DrivingManager.projectileModule.TryFire(1f);
        }

        if(Input.GetButton("Fire1"))
        {
            m_DrivingManager.driftingModule.TryStartDrifting(m_HorizontalAxis);
        }

        if(Input.GetButtonUp("Fire1"))
        {
            m_DrivingManager.driftingModule.FinishDrifting();
        }
    }

    protected virtual void FixedUpdate()
    {
        // Use the movement module to move the car
        // (You can also move the car if the network is not connected for debugging purposes)
        m_DrivingManager.Turn(m_HorizontalAxis);
        m_DrivingManager.Thrust(m_VerticalAxis);
    }
    #endregion
}
