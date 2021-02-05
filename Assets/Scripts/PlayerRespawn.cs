using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [TagSelector]
    [SerializeField]
    [Tooltip("Tag of the object that the car respawns at")]
    private string m_RespawnTag;
    [SerializeField]
    [Tooltip("If the car passes below this height, it respawns on the track")]
    private float m_PitLevel;

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

    private void Update()
    {
        if (transform.position.y < m_PitLevel)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        GameObject respawn = GameObject.FindGameObjectWithTag(m_RespawnTag);

        if (respawn != null)
        {
            rigidbody.position = respawn.transform.position;
            rigidbody.rotation = respawn.transform.rotation;

            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
