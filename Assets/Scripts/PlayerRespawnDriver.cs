using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GroundingModule))]
public class PlayerRespawnDriver : MonoBehaviour
{
    [SerializeField]
    [Tooltip("If the car passes below this height, it respawns on the track")]
    private float m_PitLevel;

    private Rigidbody m_Rigidbody;
    private GroundingModule m_GroundingModule;

    private Vector3 m_beginningOfTrack = new Vector3(-135.16f, 0.55f, 0.0086f);
    private Vector3 m_endOfTrack = new Vector3(135.80f, 0.55f, 1.45f);
    private Vector3 m_respawnLocation;
    bool falling;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_GroundingModule = GetComponent<GroundingModule>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_GroundingModule.Grounded() && !falling)
        {
            m_respawnLocation = ClosestPoint(m_beginningOfTrack, m_endOfTrack, transform.position);
            falling = true;
        }

        if (transform.position.y < m_PitLevel)
        {
            Respawn(m_respawnLocation);
            falling = false;
        }

        if (m_GroundingModule.Grounded())
        {
            falling = false;
        }
    }

    // Move the player back to the respawn location
    private void Respawn(Vector3 respawnLocation)
    {
        m_Rigidbody.position = respawnLocation + (Vector3.up * 5f);
        m_Rigidbody.rotation = Quaternion.Euler(m_Rigidbody.rotation.x, m_Rigidbody.rotation.y, 0f);

        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
    }

    private Vector3 ClosestPoint(Vector3 limit1, Vector3 limit2, Vector3 point)
    {
        Vector3 lineVector = limit2 - limit1;

        float lineVectorSqrMag = lineVector.sqrMagnitude;

        // Trivial case where limit1 == limit2
        if (lineVectorSqrMag < 1e-3f)
            return limit1;

        float dotProduct = Vector3.Dot(lineVector, limit1 - point);

        float t = -dotProduct / lineVectorSqrMag;

        return limit1 + Mathf.Clamp01(t) * lineVector;
    }
}
