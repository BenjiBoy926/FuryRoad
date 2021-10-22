using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(DrivingManager))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the movement module that this script drives")]
    private DrivingManager m_MovementModule;
    private float m_HorizontalAxis;
    private float m_VerticalAxis;

    public DrivingManager movementModule => m_MovementModule;

    protected virtual void Update()
    {
        // Setup current input axes every frame
        m_HorizontalAxis = Input.GetAxis("Horizontal");
        m_VerticalAxis = Input.GetAxis("Vertical");

        if(Input.GetButtonDown("Jump"))
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                m_MovementModule.TryFireProjectile(-1f);
            }
            else m_MovementModule.TryFireProjectile(1f);
        }

        if(Input.GetButton("Fire1"))
        {
            m_MovementModule.TryStartDrifting(m_HorizontalAxis);
        }

        if(Input.GetButtonUp("Fire1"))
        {
            m_MovementModule.FinishDrifting();
        }
    }

    protected virtual void FixedUpdate()
    {
        // Use the movement module to move the car
        // (You can also move the car if the network is not connected for debugging purposes)
        m_MovementModule.Turn(m_HorizontalAxis);
        m_MovementModule.Thrust(m_VerticalAxis);
    }
}
