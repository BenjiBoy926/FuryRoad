﻿using System.Collections;
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

    private Vector3 m_beginningOfTrack = new Vector3(-135.16f,0.55f,0.0086f);
    private Vector3 m_endOfTrack = new Vector3(135.80f,0.55f,1.45f);

    private Vector3 m_respawnLocation;
    //private Vector3 m_fallLocation;
    bool isGrounded = true;
    bool falling;

    private void Update()
    {
        m_HorizontalAxis = Input.GetAxis("Horizontal");
        m_VerticalAxis = Input.GetAxis("Vertical");
        Debug.Log("isGrounded " + isGrounded);
        Debug.Log("Falling " + falling);
        if (!isGrounded && !falling){
            Vector3 m_fallLocation = transform.position;
            m_respawnLocation = ClosestPoint(m_beginningOfTrack, m_endOfTrack, m_fallLocation);
            falling = true;
        }

        if (transform.position.y < m_PitLevel)
        {
            Respawn(m_respawnLocation);
            falling = false;
        }

        if(isGrounded){
            falling = false;
        }
        
    }

    private void FixedUpdate()
    {
        m_MovementModule.Turn(m_HorizontalAxis);
        m_MovementModule.Thrust(m_VerticalAxis);
    }

    private void Respawn(Vector3 respawnLocation)
    {
        GameObject respawn = GameObject.FindGameObjectWithTag(m_RespawnTag);


        if (respawn != null)
        {
            
            

            transform.position = respawnLocation;
            transform.rotation = respawn.transform.rotation;

            m_MovementModule.rigidbody.velocity = Vector3.zero;
            m_MovementModule.rigidbody.angularVelocity = Vector3.zero;

            falling = false;
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        if(collisionInfo.collider.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }
 
    void OnCollisionExit(Collision collisionInfo)
    {
        if(collisionInfo.collider.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }

    private Vector3 ClosestPoint(Vector3 limit1, Vector3 limit2, Vector3 point)
     {
         Vector3 lineVector = limit2 - limit1;
 
         float lineVectorSqrMag = lineVector.sqrMagnitude;
 
         // Trivial case where limit1 == limit2
         if(lineVectorSqrMag < 1e-3f)
             return limit1;
 
         float dotProduct = Vector3.Dot(lineVector, limit1 - point);
 
         float t = - dotProduct / lineVectorSqrMag;
 
         return limit1 + Mathf.Clamp01(t) * lineVector;
     }
}
