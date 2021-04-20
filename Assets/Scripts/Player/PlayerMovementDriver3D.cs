using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(MovementModule3D))]
public class PlayerMovementDriver3D : MonoBehaviourPunCallbacks
{
    private MovementModule3D m_MovementModule;
    private float m_HorizontalAxis;
    private float m_VerticalAxis;
    private bool m_Boost;
    public AudioSource drivingAudio;
    public AudioClip drivingAudioClip;
    public AudioClip idleAudioClip;
    private float audioPitch;
    public Rigidbody racerRigidBody;

    private void Start()
    {

        m_MovementModule = GetComponent<MovementModule3D>();
        //audioPitch = drivingAudio.pitch;
    }

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

            if(Input.GetButtonDown("Fire1"))
            {
                m_MovementModule.TryStartDrifting(m_HorizontalAxis);
            }

            if(Input.GetButtonUp("Fire1"))
            {
                m_MovementModule.StopDrifting();
            }
        }

        EngineAudio();

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

    private void EngineAudio(){
        if(racerRigidBody.velocity.magnitude > 0)
        {
            // Player is moving
            drivingAudio.clip = drivingAudioClip;
            drivingAudio.Play();
        }
        /*else{
            drivingAudio.clip = idleAudioClip;
            drivingAudio.Play();
        }*/
    }

    
}
