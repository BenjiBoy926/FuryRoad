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
    private Vector2 m_ProjectileAxis;
    #endregion

    #region Monobehaviour Messages
    private void Awake()
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

        // Setup the drift
        if (Input.GetButton("Drift"))
        {
            m_DrivingManager.driftingModule.TryStartDrifting(m_HorizontalAxis);
        }
        if(Input.GetButtonUp("Drift"))
        {
            m_DrivingManager.driftingModule.FinishDrifting();
        }

        // Setup the projectile axis
        m_ProjectileAxis.x = Input.GetAxis("ProjectileHorizontal");
        m_ProjectileAxis.y = Input.GetAxis("ProjectileVertical");

        if (Input.GetKeyDown(KeyCode.W))
        {
            m_DrivingManager.projectileModule.TryFire(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            m_DrivingManager.projectileModule.TryFire(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_DrivingManager.projectileModule.TryFire(Vector2.left);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            m_DrivingManager.projectileModule.TryFire(Vector2.right);
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
