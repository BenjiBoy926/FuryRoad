using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DrivingManager : MonoBehaviour
{
    #region Public Typedefs
    [System.Serializable]
    public class IntEvent : UnityEvent<int> { }
    [System.Serializable]
    public class RacingLapDataEvent : UnityEvent<RacingLapData> { }
    [System.Serializable]
    public class DrivingManagerEvent : UnityEvent<DrivingManager> { }
    [System.Serializable]
    public class ProjectileEvent : UnityEvent<Projectile> { }
    #endregion

    #region Public Properties
    // Public getters
    public new Rigidbody rigidbody => m_Rigidbody;
    public GroundingModule groundingModule => m_GroundingModule;
    public TopSpeedModule topSpeedModule => m_TopSpeedModule;
    public BoostingResources boostResources => m_BoostResources;
    public BoostingModule boostingModule => m_BoostingModule;
    public DriftingModule driftingModule => m_DriftingModule;
    public DraftingModule draftingModule => m_DraftingModule;
    public ProjectileModule projectileModule => m_ProjectileModule;
    public Vector3 heading => m_Heading;
    // Speed that the car is driving at (excludes fall speed, only in the plane we are driving in)
    public float drivingSpeed => Vector3.ProjectOnPlane(m_Rigidbody.velocity, m_GroundingModule.groundNormal).magnitude;
    public UnityEvent<DrivingManager> DriverRegisteredEvent => driverRegisteredEvent;
    public UnityEvent<DrivingManager> DriverDeregisteredEvent => driverDeregisteredEvent;
    public UnityEvent<RacingLapData> NewLapEvent => newLapEvent;
    public UnityEvent<int> PlayerFinishedEvent => playerFinishedEvent;
    public UnityEvent<DrivingManager> ProjectileHitOtherEvent => projectileHitOtherEvent;
    public UnityEvent<Projectile> ProjectileHitMeEvent => projectileHitMeEvent;
    // Rotate heading around the ground to get the right
    public Vector3 right => Quaternion.AngleAxis(90f, m_GroundingModule.groundNormal) * m_Heading;
    public Vector3 up => m_GroundingModule.groundNormal;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Referene to the sphere rigidbody that moves the car around")]
    private Rigidbody m_Rigidbody;
    [SerializeField]
    [Tooltip("Strength of gravity's pull on the movement module")]
    private float gravity = -9.81f;
    [SerializeField]
    [Tooltip("Power of the racer's acceleration")]
    private float m_Thrust = 10f;
    [SerializeField]
    [Tooltip("Tightness of the racer's turn")]
    private float m_Turn = 10f;

    [Space]

    [SerializeField]
    [Tooltip("Reference to the module used to determine if this object is on the ground")]
    private GroundingModule m_GroundingModule;
    [SerializeField]
    [Tooltip("Manage the top speed of the racer")]
    private TopSpeedModule m_TopSpeedModule;
    [SerializeField]
    [Tooltip("Used to manage the boosting resources of the vehicle")]
    private BoostingResources m_BoostResources;
    [SerializeField]
    [Tooltip("Module with the information on how to boost")]
    private BoostingModule m_BoostingModule;
    [SerializeField]
    [Tooltip("Module with the information on how to drift")]
    private DriftingModule m_DriftingModule;
    [SerializeField]
    [Tooltip("Reference to the script that handles the drafting of the car")]
    private DraftingModule m_DraftingModule;
    [SerializeField]
    [Tooltip("Reference to the object used to fire projectiles")]
    private ProjectileModule m_ProjectileModule;

    [Space]

    [SerializeField]
    [Tooltip("Event invoked when a new player enters the scene that this driver is in")]
    private DrivingManagerEvent driverRegisteredEvent;
    [SerializeField]
    [Tooltip("Event invoked when any values are pruned in the driver registry")]
    private DrivingManagerEvent driverDeregisteredEvent;
    [SerializeField]
    [Tooltip("Event invoked when the player manager finishes a new lap")]
    private RacingLapDataEvent newLapEvent;
    [SerializeField]
    [Tooltip("Event invoked when the player finishes the race")]
    private IntEvent playerFinishedEvent;
    [SerializeField]
    [Tooltip("Event invoked when my projectile hits another driver")]
    private DrivingManagerEvent projectileHitOtherEvent;
    [SerializeField]
    [Tooltip("Event invoked when another projectile hit me")]
    private ProjectileEvent projectileHitMeEvent;
    #endregion

    #region Private Fields
    // Current heading of the movement module
    private Vector3 m_Heading = Vector3.forward;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        driftingModule.driftStartEvent.AddListener(OnDriftStarted);
        driftingModule.driftStopEvent.AddListener(OnDriftStopped);

        // Register this driver with the registry
        DriverRegistry.Register(this);
    }
    private void FixedUpdate()
    {
        Vector3 groundNormal = m_GroundingModule.groundNormal;

        // If the heading is at an angle with the ground normal, re-assign the heading of the movement module
        if (Mathf.Abs(Vector3.Dot(m_Heading, groundNormal)) > Mathf.Epsilon)
        {
            m_Heading = Vector3.ProjectOnPlane(m_Heading, groundNormal).normalized;
        }

        // Gravity pulls against the ground normal, it will not pull directly down!
        // This is the only way the sphere can naturally drive on an inclined surface,
        // otherwise it cannot fight its own weight to work up the incline
        m_Rigidbody.AddForce(m_GroundingModule.groundNormal * gravity, ForceMode.Acceleration);

        // Clamp the velocity magnitude within the top speed
        m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_TopSpeedModule.currentTopSpeed);
    }
    private void OnDestroy()
    {
        DriverRegistry.Deregister(this);
    }
    #endregion

    #region Public Methods
    public void Turn(float horizontal)
    {
        // Car can only turn while moving and grounded, and while the script is enabled
        if (m_Rigidbody.velocity.sqrMagnitude > 0.1f && m_GroundingModule.grounded)
        {
            // Let the drifting module decide how we will actually steer the car
            horizontal = m_DriftingModule.GetSteer(horizontal);

            // Define a rotation around the ground normal
            float rotationAngle = horizontal * m_Turn * Time.fixedDeltaTime;

            // TODO: Verify that this ensures the car properly turns when driving
            // backwards
            if (Vector3.Dot(heading, m_Rigidbody.velocity) < 0f) { rotationAngle *= -1f; }

            Quaternion rotation = Quaternion.AngleAxis(rotationAngle, groundingModule.groundNormal);

            // Rotate the rigidbody, the velocity, and the heading
            m_Rigidbody.velocity = rotation * m_Rigidbody.velocity;
            m_Rigidbody.angularVelocity = rotation * m_Rigidbody.angularVelocity;
            m_Heading = rotation * m_Heading;
        }
    }
    public void Thrust(float vertical)
    {
        // Car can only thrust while grounded and the manager is enabled
        if(m_GroundingModule.grounded)
        {
            // Get the heading of the vehicle according to the drifting module,
            // because the drifting module rotates the true heading
            //Vector3 heading = driftingModule.GetHeading();
            m_Rigidbody.AddForce(heading * vertical * m_Thrust * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
    public void SetHeading(Vector3 heading)
    {
        m_Heading = heading;
    }
    public void OnNewLap(RacingLapData data)
    {
        newLapEvent.Invoke(data);
    }
    public void OnRaceFinished(int rank)
    {
        playerFinishedEvent.Invoke(rank);
    }
    #endregion

    #region Private Methods
    private void OnDriftStarted()
    {
        // When the drift begins, immediate set the speed to go in the direction of the rotated heading
        // instead of the true heading
        Vector3 verticalComponent = Vector3.Project(m_Rigidbody.velocity, groundingModule.groundNormal);
        Vector3 drivingComponent = driftingModule.GetHeading().normalized * drivingSpeed;
        m_Rigidbody.velocity = drivingComponent + verticalComponent;
    }
    private void OnDriftStopped()
    {
        // When the drift begins, immediate set the speed to go in the direction of the rotated heading
        // instead of the true heading
        Vector3 verticalComponent = Vector3.Project(m_Rigidbody.velocity, groundingModule.groundNormal);
        Vector3 drivingComponent = heading.normalized * drivingSpeed;
        m_Rigidbody.velocity = drivingComponent + verticalComponent;
    }
    #endregion
}
