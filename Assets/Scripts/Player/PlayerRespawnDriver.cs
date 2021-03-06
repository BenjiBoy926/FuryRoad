﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundingModule))]
public class PlayerRespawnDriver : MonoBehaviour
{
    [SerializeField]
    [Tooltip("If the car passes below this height, it respawns on the track")]
    private float m_PitLevel;
    [SerializeField]
    [Tooltip("Rigidbody to move when we respawn")]
    private Rigidbody m_Rigidbody;


    private GroundingModule m_GroundingModule;


    public Vector3 closestCheckPoint;

    private Vector3 m_respawnLocation;
    bool falling;

    private void Start()
    {
        m_GroundingModule = GetComponent<GroundingModule>();
        
    }

    private void FixedUpdate()
    {
        collisionDetection collisionScript = m_Rigidbody.GetComponent<collisionDetection>();
        closestCheckPoint = collisionScript.closestCheckPoint;
        // Store the result of the function since we use it multiple times
        bool grounded = m_GroundingModule.grounded;

        if (!grounded && !falling)
        {
            m_respawnLocation = closestCheckPoint;
            falling = true;
        }

        if (m_Rigidbody.position.y < m_PitLevel)
        {
            Respawn(m_respawnLocation);
            falling = false;
        }

        if (grounded)
        {
            falling = false;
        }
    }

    // Move the player back to the respawn location
    private void Respawn(Vector3 respawnLocation)
    {
        m_Rigidbody.position = respawnLocation + (Vector3.up * 5f);
        m_Rigidbody.rotation = ComputeTrackForward();

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

    // Use this function to determine how to rotate the car so it faces "forward" on the track
    private Quaternion ComputeTrackForward()
    {
        return Quaternion.LookRotation(transform.forward, Vector3.up);
    }

}
