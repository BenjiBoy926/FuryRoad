using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDriving : MonoBehaviour
{
    #region Public Properties
    public DrivingManager drivingManager => m_DrivingManager;
    #endregion

    #region Public Fields
    public readonly VirtualAction<bool> setControl = new VirtualAction<bool>();
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
    private void Start()
    {
        setControl.SetVirtual(enabled =>
        {
            this.enabled = enabled;

            // Make sure the drafting module is enabled/disabled so that
            // a car without controls cannot draft
            m_DrivingManager.draftingModule.enabled = enabled;
        });
    }
    protected virtual void Update()
    {
        // Setup current input axes every frame
        m_HorizontalAxis = Input.GetAxis("Horizontal");
        m_VerticalAxis = Input.GetAxis("Drive");

        // Jump button fires projectiles
        if(Input.GetButtonDown("Action"))
        {
            // If axis less than zero, or down keys pressed on laptop standalone, then fire a projectile
            if (Input.GetAxis("Vertical") < -0.2 || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                m_DrivingManager.projectileModule.TryFire(-1f);
            }
            else m_DrivingManager.projectileModule.TryFire(1f);
        }
        if (Input.GetButton("Drift"))
        {
            m_DrivingManager.driftingModule.TryStartDrifting(m_HorizontalAxis);
        }
        if(Input.GetButtonUp("Drift"))
        {
            m_DrivingManager.driftingModule.FinishDrifting();
        }
    }

    protected virtual void FixedUpdate()
    {
        // Use the movement module to move the car
        m_DrivingManager.Turn(m_HorizontalAxis);
        m_DrivingManager.Thrust(m_VerticalAxis);
    }
    #endregion
}
