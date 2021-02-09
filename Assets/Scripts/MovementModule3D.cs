using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Components required
    private Rigidbody m_Rigidbody;
    private GroundingModule m_GroundingModule;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_GroundingModule = GetComponent<GroundingModule>();
    }

    public void Turn(float horizontal)
    {
        // Car can only turn while moving and grounded
        if(m_Rigidbody.velocity.sqrMagnitude > 0.1f && m_GroundingModule.Grounded())
        {
            Quaternion rotation = Quaternion.Euler(0f, horizontal * m_Turn * Time.fixedDeltaTime, 0f);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * rotation);
            m_Rigidbody.velocity = rotation * m_Rigidbody.velocity;
        }
    }

    public void Thrust(float vertical)
    {
        // Car can only thrust while grounded
        if(m_GroundingModule.Grounded())
        {
            m_Rigidbody.AddRelativeForce(Vector3.forward * vertical * m_Thrust * Time.fixedDeltaTime, ForceMode.VelocityChange);
            m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_TopSpeed);
        }
    }

    public void CleanUp()
    {
        // If the car is facing slightly upside-down, correct it to face right side up again and remove angular velocity
        if (Vector3.Dot(transform.up, Vector3.down) > 0)
        {
            m_Rigidbody.rotation = Quaternion.AngleAxis(0f, transform.forward);
            m_Rigidbody.position += Vector3.up * 5f;
            //m_Rigidbody.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(m_Rigidbody.velocity, Vector3.up));
            m_Rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
