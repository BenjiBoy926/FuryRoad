using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GroundingModule))]
public class MovementModule3D : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Power of the racer's acceleration")]
    private float m_Thrust = 10f;
    [SerializeField]
    [Tooltip("Tightness of the racer's turn")]
    private float m_Turn = 10f;
    [SerializeField]
    [Tooltip("Maximum speed of the racer")]
    private float m_TopSpeed = 30f;
    [SerializeField]
    [Tooltip("Strength of the force that moves the rotation towards the heading")]
    private float m_RotationStrength = 100f;

    // Components required
    private Rigidbody m_Rigidbody;
    private GroundingModule m_GroundingModule;

    public Vector3 heading
    {
        get; private set;
    }

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_GroundingModule = GetComponent<GroundingModule>();

        // Initialize the heading to the initial forward vector, projected onto the x-z plane
        heading = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
    }

    public void Turn(float horizontal)
    {
        Quaternion rotation = Quaternion.Euler(0f, horizontal * m_Turn * Time.fixedDeltaTime, 0f);

        // Rotate the velocity, rotation, and heading
        m_Rigidbody.velocity = rotation * m_Rigidbody.velocity;
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * rotation);
        heading = rotation * heading;
    }

    public void Thrust(float vertical)
    {
        // Thrust the car forward
        m_Rigidbody.AddForce(heading * vertical * m_Thrust * Time.fixedDeltaTime, ForceMode.VelocityChange);
        m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_TopSpeed);

        // While the car thrusts, move the rotation towards the heading
        if(Mathf.Abs(vertical) > 0f)
        {
            Quaternion goalRotation = Quaternion.LookRotation(heading, Vector3.up);
            goalRotation = Quaternion.RotateTowards(m_Rigidbody.rotation, goalRotation, m_RotationStrength * Time.fixedDeltaTime);
            m_Rigidbody.MoveRotation(goalRotation);
        }
    }

    public void CleanUp()
    {
        // If the car is facing slightly upside-down, correct it to face right side up again and remove angular velocity
        if (Vector3.Dot(transform.up, Vector3.down) > 0)
        {
            m_Rigidbody.rotation = Quaternion.AngleAxis(0f, transform.forward);
            m_Rigidbody.position += Vector3.up * 5f;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
