using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashingModule : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Force to exert on the driver when it hits another driver")]
    private float crashStrength;
    [SerializeField]
    [Tooltip("Particle system to activate when a crash occurs")]
    private ParticleSystem crashParticles;
    [SerializeField]
    [Tooltip("Audio source to play when a crash occurs")]
    private AudioSource crashAudio;
    [SerializeField]
    [Tooltip("List of objects that can detect collisions for the driver")]
    private CollisionEvents[] colliders;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        foreach (CollisionEvents collider in colliders)
        {
            collider.CollisionEnter.AddListener(OnDriverCollisionEnter);
            collider.TriggerEnter.AddListener(OnDriverTriggerEnter);
        }
    }
    #endregion

    #region Event Listeners
    private void OnDriverCollisionEnter(Collision collision)
    {
        DriverCrash(collision.gameObject.GetComponentInParent<DrivingManager>());
    }
    private void OnDriverTriggerEnter(Collider collider)
    {
        DriverCrash(collider.GetComponent<DrivingManager>());
    }
    #endregion

    #region Public Methods
    public void DriverCrash(DrivingManager otherDriver)
    {
        if (otherDriver != null && otherDriver != manager)
        {
            Debug.Log("Crashed into the driver!");
        }
    }
    #endregion
}
