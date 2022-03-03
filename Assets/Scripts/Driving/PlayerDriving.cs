using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDriving : MonoBehaviour
{
    #region Public Properties
    public DrivingManager drivingManager => m_DrivingManager;
    public Vector2 projectileAxis => m_ProjectileAxis;
    public bool usingJoystickAxis => m_UsingJoystickAxis;
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
    private bool m_UsingJoystickAxis = false;
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
        Vector2 currentAxis = new Vector2(
            Input.GetAxis("ProjectileHorizontal"),
            Input.GetAxis("ProjectileVertical"));

        // Use the joystick axis if we got an input from it
        m_UsingJoystickAxis = currentAxis.sqrMagnitude > 0.1f;

        // If we are not using the joystick axis then use the mouse direction instead
        if (!m_UsingJoystickAxis)
        {
            currentAxis = ComputeMouseDirection();
        }

        // Assign the axis if it has a magnitude,
        // otherwise keep the axis from the previous update
        if (currentAxis.sqrMagnitude > 0.1f) m_ProjectileAxis = currentAxis;

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

    #region Public Methods
    public Vector3 ComputeMousePosition()
    {
        // Shoot a ray out from the camera
        Ray camRay = driverCam.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(camRay, out RaycastHit hitInfo, 100f, environmentLayer);

        // Check if there was a hit
        if (hit)
        {
            return hitInfo.point;
        }
        else return Vector3.zero;
    }
    public Vector2 ComputeMouseDirection()
    {
        // Compute mouse ray position
        Vector3 mousePosition = ComputeMousePosition();

        // If we got a position then return the magnitude
        if (mousePosition.sqrMagnitude > 0.1f)
        {
            mousePosition -= m_DrivingManager.rigidbody.position;
            mousePosition = m_DrivingManager.TransformPoint(mousePosition);
            mousePosition = mousePosition.normalized;
            return new Vector2(mousePosition.x, mousePosition.z);
        }
        else return Vector2.zero;
    }
    #endregion
}
