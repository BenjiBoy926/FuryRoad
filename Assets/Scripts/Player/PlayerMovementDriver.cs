using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(MovementManager))]
public class PlayerMovementDriver : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the photon view that is attached to the player movement sphere")]
    private PhotonView photonView;
    [SerializeField]
    [Tooltip("Reference to the movement module that this script drives")]
    private MovementManager m_MovementModule;
    private float m_HorizontalAxis;
    private float m_VerticalAxis;

    public MovementManager movementModule => m_MovementModule;

    private void Update()
    {
        // Do not setup input axes unless this photon view is mine
        // (You can also contorl the car if the network is not connected for debugging purposes)
        if(photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            m_HorizontalAxis = Input.GetAxis("Horizontal");
            m_VerticalAxis = Input.GetAxis("Vertical");

            if(Input.GetButtonDown("Jump"))
            {
                m_MovementModule.TryStartBoost();
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
    }

    private void FixedUpdate()
    {
        // Use the movement module to move the car
        // (You can also move the car if the network is not connected for debugging purposes)
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            m_MovementModule.Turn(m_HorizontalAxis);
            m_MovementModule.Thrust(m_VerticalAxis);
        }
    }
}
