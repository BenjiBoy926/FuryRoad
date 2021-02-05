using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementModule3D
{
    [SerializeField]
    [Tooltip("The rigidbody to move")]
    private Rigidbody m_Rigidbody;
    [SerializeField]
    [Tooltip("Power of the racer's acceleration")]
    private float m_Thrust = 10f;
    [SerializeField]
    [Tooltip("Tightness of the racer's turn")]
    private float m_Turn = 10f;
    [SerializeField]
    [Tooltip("Maximum speed of the racer")]
    private float m_TopSpeed = 30f;

    public Rigidbody rigidbody
    {
        get
        {
            return m_Rigidbody;
        }
    }

    public MovementModule3D(Rigidbody rb, float thrust, float turn)
    {
        m_Rigidbody = rb;
        m_Thrust = thrust;
        m_Turn = turn;
    }

    public void Turn(float horizontal)
    {
        // Car can only turn while moving
        if(m_Rigidbody.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.Euler(0f, horizontal * m_Turn * Time.fixedDeltaTime, 0f);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * rotation);
            m_Rigidbody.velocity = rotation * m_Rigidbody.velocity;
        }
    }

    public void Thrust(float vertical)
    {
        m_Rigidbody.AddRelativeForce(Vector3.forward * vertical * m_Thrust);
        m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_TopSpeed);
    }
}
