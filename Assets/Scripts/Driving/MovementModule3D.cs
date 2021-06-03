using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GroundingModule))]
public class MovementModule3D : MonoBehaviour
{
    [Header("Driving")]

    [SerializeField]
    [Tooltip("Referene to the sphere rigidbody that moves the car around")]
    private Rigidbody m_Rigidbody;
    [SerializeField]
    [Tooltip("Reference to the script that manages player UI")]
    private PlayerUIManager ui;
    [SerializeField]
    [Tooltip("Strength of gravity's pull on the movement module")]
    private float gravity = -9.81f;
    [SerializeField]
    [Tooltip("Power of the racer's acceleration")]
    private float m_Thrust = 10f;
    [SerializeField]
    [Tooltip("Tightness of the racer's turn")]
    private float m_Turn = 10f;
    [SerializeField]
    [Tooltip("Manage the top speed of the racer")]
    private TopSpeedModule m_TopSpeedModule;

    [Header("Boosting")]

    [SerializeField]
    [Tooltip("Used to manage the boosting resources of the vehicle")]
    private BoostingResources m_BoostResources;
    [SerializeField]
    [Tooltip("Module with the information on how to boost")]
    private BoostingModule m_BoostingModule;

    [Header("Drifting")]

    [SerializeField]
    [Tooltip("Module with the information on how to drift")]
    private DriftingModule m_DriftingModule;

    [Header("Drafting")]

    [SerializeField]
    [Tooltip("Reference to the script that handles the drafting of the car")]
    private DraftingModule m_DraftingModule;

    [Header("Terrain")]

    [SerializeField]
    [Tooltip("Reference to the module that modifies the car's speed over different terrain types")]
    private TerrainModule m_TerrainModule;

    [SerializeField]
    [Tooltip("Reference to the script that handles the finish line")]
    private FinishLine finishLine;

    // Components required
    private GroundingModule m_GroundingModule;
    // Current heading of the movement module
    private Vector3 _heading;

    // Public getters
    public new Rigidbody rigidbody => m_Rigidbody;
    public BoostingModule boostingModule => m_BoostingModule;
    public DriftingModule driftingModule => m_DriftingModule;
    public GroundingModule groundingModule => m_GroundingModule;
    public Vector3 heading => _heading;

    private void Awake()
    {
        m_GroundingModule = GetComponent<GroundingModule>();
        _heading = Vector3.forward;

        m_TopSpeedModule.Setup(m_BoostingModule, m_DriftingModule.driftBoost, m_DraftingModule, m_TerrainModule);
        m_BoostingModule.Awake();
        m_DriftingModule.Awake();
        m_BoostResources.Awake();
    }

    private void FixedUpdate()
    {
        Vector3 groundNormal = m_GroundingModule.groundNormal;

        // If the heading is at an angle with the ground normal, re-assign the heading of the movement module
        if (Mathf.Abs(Vector3.Dot(_heading, groundNormal)) > Mathf.Epsilon)
        {
            _heading = Vector3.ProjectOnPlane(_heading, groundNormal).normalized;
        }

        // Gravity pulls against the ground normal, it will not pull directly down!
        // This is the only way the sphere can naturally drive on an inclined surface,
        // otherwise it cannot fight its own weight to work up the incline
        m_Rigidbody.AddForce(m_GroundingModule.groundNormal * gravity, ForceMode.Acceleration);

        // Update all submodules
        m_TopSpeedModule.FixedUpdate();
        m_BoostResources.FixedUpdate(m_DriftingModule.driftActive, false, !groundingModule.grounded);
        m_BoostingModule.FixedUpdate(m_Rigidbody, m_TopSpeedModule.currentTopSpeed, heading, groundingModule.groundNormal);
        m_DriftingModule.FixedUpdate(m_Rigidbody, m_TopSpeedModule.currentTopSpeed, heading, groundingModule.groundNormal);
        m_DraftingModule.FixedUpdate(m_Rigidbody, heading);
        m_TerrainModule.FixedUpdate(m_GroundingModule);

        // Clamp the velocity magnitude within the top speed
        m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_TopSpeedModule.currentTopSpeed);
        ui.UpdateSpeedUI(m_Rigidbody.velocity.magnitude);

        // A temporary fix, since we are getting null references
        if(finishLine != null) ui.UpdatePlacementUI(finishLine.GetLocalPlayerRanking());
    }
    public void Turn(float horizontal)
    {
        // Car can only turn while moving and grounded
        if(m_Rigidbody.velocity.sqrMagnitude > 0.1f && m_GroundingModule.grounded)
        {
            // Let the drifting module decide how we will actually steer the car
            horizontal = m_DriftingModule.GetSteer(horizontal);

            // Define a rotation around the y axis
            Quaternion rotation = Quaternion.Euler(0f, horizontal * m_Turn * Time.fixedDeltaTime, 0f);
            
            // Rotate the rigidbody, the velocity, and the heading
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * rotation);
            m_Rigidbody.velocity = rotation * m_Rigidbody.velocity;
            m_Rigidbody.angularVelocity = rotation * m_Rigidbody.angularVelocity;
            _heading = rotation * _heading;
        }
    }

    public void Thrust(float vertical)
    {
        // Car can only thrust while grounded
        if(m_GroundingModule.grounded && !m_BoostingModule.boostActive)
        {
            m_Rigidbody.AddForce(_heading * vertical * m_Thrust * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

    // Delegates for the boosting module
    public bool TryStartBoost()
    {
        // If we have resources for a boost, then try to boost
        if (m_BoostResources.canBoost)
        {
            // Try to start boosting and store the result of the attempt
            bool result = m_BoostingModule.TryStartBoosting(m_GroundingModule, m_Rigidbody, m_TopSpeedModule.currentTopSpeed, _heading);

            // If we started boosting, then consume a boost resource
            if (result) m_BoostResources.ConsumeBoostResource();

            // Return true/false if we are now boosting or not
            return result;
        }
        else return false;
    }
    // Delegates for the drifting module
    public bool TryStartDrifting(float h)
    {
        return m_DriftingModule.TryStartDrifting(m_GroundingModule, h);
    }
    public void StartDrifting(float h)
    {
        m_DriftingModule.StartDrifting(h);
    }
    public void FinishDrifting()
    {
        m_DriftingModule.FinishDrifting(m_Rigidbody, m_TopSpeedModule.currentTopSpeed, heading);
    }

    public void SetHeading(Vector3 heading)
    {
        _heading = heading;
        rigidbody.transform.forward = heading;
    }
}
