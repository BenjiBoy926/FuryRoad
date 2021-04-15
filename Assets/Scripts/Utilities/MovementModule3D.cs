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
    [Tooltip("Module with the information on how to boost")]
    private BoostingModule m_BoostingModule;
    [SerializeField]
    [Tooltip("Module with the information on how to drift")]
    private DriftingModule m_DriftingModule;

    // Components required
    private Rigidbody m_Rigidbody;
    private GroundingModule m_GroundingModule;

    // Public getters
    public BoostingModule boostingModule => m_BoostingModule;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_GroundingModule = GetComponent<GroundingModule>();
    }

    private void Update()
    {
        m_BoostingModule.Update();
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
        if(m_GroundingModule.Grounded() && !m_BoostingModule.boostActive)
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
            m_Rigidbody.rotation = Quaternion.AngleAxis(0f, transform.right);
            m_Rigidbody.position += Vector3.up * 5f;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }
    }
    public bool TryStartBoost()
    {
        return m_BoostingModule.TryStartBoosting(m_GroundingModule, m_Rigidbody, m_TopSpeed);
    }
    public void StartBoost()
    {
        m_BoostingModule.StartBoosting(m_Rigidbody, m_TopSpeed);
    }
}
