using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    private MovementModule3D m_MovementModule;
    private float m_HorizontalAxis;
    private float m_VerticalAxis;

    public MovementModule3D movementModule
    {
        get
        {
            if(m_MovementModule == null)
            {
                m_MovementModule = GetComponent<MovementModule3D>();
            }
            return m_MovementModule;
        }
    }

    private void Update()
    {
        m_HorizontalAxis = Input.GetAxis("Horizontal");
        m_VerticalAxis = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        movementModule.Turn(m_HorizontalAxis);
        movementModule.Thrust(m_VerticalAxis);
        movementModule.CleanUp();
    }
}
