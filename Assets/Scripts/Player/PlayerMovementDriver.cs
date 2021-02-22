using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(VehicleMovementModule))]
public class PlayerMovementDriver: MonoBehaviourPunCallbacks
{
    private VehicleMovementModule m_MovementModule;
    private float m_HorizontalAxis;
    private float m_VerticalAxis;

    private void Start()
    {
        m_MovementModule = GetComponent<VehicleMovementModule>();
    }

    private void Update()
    {
        // Do not setup input axes unless this photon view is mine
        // (You can also contorl the car if the network is not connected for debugging purposes)
        if(photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            m_HorizontalAxis = Input.GetAxis("Horizontal");
            m_VerticalAxis = Input.GetAxis("Vertical");
        }
    }

    private void FixedUpdate()
    {
        // Use the movement module to move the car
        // (You can also move the car if the network is not connected for debugging purposes)
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            m_MovementModule.Drive(m_HorizontalAxis);
            m_MovementModule.Steer(m_VerticalAxis);
            //m_MovementModule.CleanUp();
        }
    }

    
}
