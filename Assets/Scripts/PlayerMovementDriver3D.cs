using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementModule3D))]
public class PlayerMovementDriver3D : MonoBehaviour
{
    private MovementModule3D m_MovementModule;
    private float m_HorizontalAxis;
    private float m_VerticalAxis;

    private void Start()
    {
        m_MovementModule = GetComponent<MovementModule3D>();
    }

    private void Update()
    {
        m_HorizontalAxis = Input.GetAxis("Horizontal");
        m_VerticalAxis = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        m_MovementModule.Turn(m_HorizontalAxis);
        m_MovementModule.Thrust(m_VerticalAxis);
        m_MovementModule.CleanUp();
    }
}
