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
    [Tooltip("Strength of gravity's pull on the movement module")]
    private float gravity = -9.81f;
    [SerializeField]
    [Tooltip("Power of the racer's acceleration")]
    private float m_Thrust = 10f;
    [SerializeField]
    [Tooltip("Tightness of the racer's turn")]
    private float m_Turn = 10f;
    [SerializeField]
    [Tooltip("Maximum speed of the racer")]
    private float m_TopSpeed = 30f;

    [Header("Boosting")]

    [SerializeField]
    [Tooltip("Module with the information on how to boost")]
    private BoostingModule m_BoostingModule;

    [Header("Drifting")]

    [SerializeField]
    [Tooltip("Module with the information on how to drift")]
    private DriftingModule m_DriftingModule;

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

    private void Start()
    {
        m_GroundingModule = GetComponent<GroundingModule>();
        _heading = Vector3.forward;

        m_BoostingModule.Start();
        m_DriftingModule.Start();
    }

    private void FixedUpdate()
    {
        Vector3 groundNormal = m_GroundingModule.groundNormal;

        // If the heading is at an angle with the ground normal, re-assign the heading of the movement module
        if (Mathf.Abs(Vector3.Dot(_heading, groundNormal)) > Mathf.Epsilon)
        {
            _heading = Vector3.ProjectOnPlane(_heading, groundNormal).normalized;
        }

        Debug.DrawRay(transform.position, heading * 10f);

        //if(m_Rigidbody.velocity.magnitude >= m_TopSpeed - 0.001f)
        //{
        //    Debug.Log("Top speed reached!");
        //}

        // Gravity pulls against the ground normal, it will not pull directly down!
        // This is the only way the sphere can naturally drive on an inclined surface,
        // otherwise it cannot fight its own weight to work up the incline
        m_Rigidbody.AddForce(m_GroundingModule.groundNormal * gravity, ForceMode.Acceleration);
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

        // Update the drift
        m_DriftingModule.FixedUpdate(heading);
    }

    public void Thrust(float vertical)
    {
        // Car can only thrust while grounded
        if(m_GroundingModule.grounded && !m_BoostingModule.boostActive)
        {
            m_Rigidbody.AddForce(_heading * vertical * m_Thrust * Time.fixedDeltaTime, ForceMode.VelocityChange);
            m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_TopSpeed);
        }

        // Update the boosting module
        m_BoostingModule.FixedUpdate(heading);
    }

    // Delegates for the boosting module
    public bool TryStartBoost()
    {
        return m_BoostingModule.TryStartBoosting(m_GroundingModule, m_Rigidbody, m_TopSpeed, heading);
    }
    public void StartBoost()
    {
        m_BoostingModule.StartBoosting(m_Rigidbody, m_TopSpeed, heading);
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
    public void StopDrifting()
    {
        m_DriftingModule.StopDrifting(m_Rigidbody, m_TopSpeed, heading);
    }
}
