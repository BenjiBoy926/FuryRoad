using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Movement information for the car")]
    private MovementModule3D m_MovementModule;
    [TagSelector]
    [SerializeField]
    [Tooltip("Tag of the object that the car respawns at")]
    private string m_RespawnTag;
    [SerializeField]
    [Tooltip("If the car passes below this height, it respawns on the track")]
    private float m_PitLevel;

    private float m_HorizontalAxis;
    private float m_VerticalAxis;

    private void Update()
    {
        m_HorizontalAxis = Input.GetAxis("Horizontal");
        m_VerticalAxis = Input.GetAxis("Vertical");

        if (transform.position.y < m_PitLevel)
        {
            Respawn();
        }
    }

    private void FixedUpdate()
    {
        m_MovementModule.Turn(m_HorizontalAxis);
        m_MovementModule.Thrust(m_VerticalAxis);
    }

    private void Respawn()
    {
        GameObject respawn = GameObject.FindGameObjectWithTag(m_RespawnTag);

        if (respawn != null)
        {
            transform.position = respawn.transform.position;
            transform.rotation = respawn.transform.rotation;

            m_MovementModule.rigidbody.velocity = Vector3.zero;
            m_MovementModule.rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
