using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDriving : MonoBehaviour
{
    #region Public Properties
    public DrivingManager drivingManager => m_DrivingManager;
    public Vector2 projectileAxis => m_ProjectileAxis;
    #endregion

    #region Public Fields
    public readonly VirtualAction<bool> setControl = new VirtualAction<bool>();
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the movement module that this script drives")]
    private DrivingManager m_DrivingManager;
    [SerializeField]
    [Tooltip("Reference to the camera used to compute the mouse's direction")]
    private Camera driverCam;
    [SerializeField]
    [Tooltip("Environment layer used to find the mouse's direction")]
    private LayerMask environmentLayer;
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

        // If the projectile axis is low, then use the mouse to compute it
        if (m_ProjectileAxis.sqrMagnitude < 0.1f)
        {
            m_ProjectileAxis = ComputeMouseDirection();
        }

        // If the button is pressed then fire a projectile
        if (Input.GetButtonDown("ProjectileFire"))
        {
            m_DrivingManager.projectileModule.TryFire(m_ProjectileAxis);
        }
    }

    protected virtual void FixedUpdate()
    {
        // Use the movement module to move the car
        m_DrivingManager.Turn(m_HorizontalAxis);
        m_DrivingManager.Thrust(m_VerticalAxis);
    }
    #endregion

    #region Private Methods
    private Vector2 ComputeMouseDirection()
    {
        // Shoot a ray out from the camera
        Ray camRay = driverCam.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(camRay, out RaycastHit hitInfo, 100f, environmentLayer);

        // Check if there was a hit
        if (hit)
        {
            // Transform local position of hit to driver coordinates
            Vector3 toHit = hitInfo.point - m_DrivingManager.rigidbody.position;
            Debug.DrawRay(m_DrivingManager.rigidbody.position, toHit);
            toHit = m_DrivingManager.TransformPoint(toHit);
            toHit = toHit.normalized;
            return new Vector2(toHit.x, toHit.z);
        }
        else return m_ProjectileAxis;
    }
    #endregion
}
