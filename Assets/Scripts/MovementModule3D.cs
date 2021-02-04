using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
    [Tooltip("Module used to detect if the car is touching the ground")]
    private GroundingModule groundingModule;

    private Rigidbody m_Rigidbody;

    public new Rigidbody rigidbody
    {
        get
        {
            if(m_Rigidbody == null)
            {
                m_Rigidbody = GetComponent<Rigidbody>();
            }
            return m_Rigidbody;
        }
    }

    public void Turn(float horizontal)
    {
        // Car can only turn while moving and while on the ground
        if(rigidbody.velocity.sqrMagnitude > 0.1f && groundingModule.Grounded(transform))
        {
            Quaternion rotation = Quaternion.Euler(0f, horizontal * m_Turn * Time.fixedDeltaTime, 0f);
            rigidbody.MoveRotation(rigidbody.rotation * rotation);
            rigidbody.velocity = rotation * rigidbody.velocity;
        }
    }

    public void Thrust(float vertical)
    {
        // Car only has thrust while on the ground
        if(groundingModule.Grounded(transform))
        {
            rigidbody.AddRelativeForce(Vector3.forward * vertical * m_Thrust * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, m_TopSpeed);
        }
    }
}
