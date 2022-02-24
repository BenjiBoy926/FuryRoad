using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashingModule : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Speed of the driver when it hits another driver")]
    private float crashSpeed;
    [SerializeField]
    [Tooltip("Particle system to activate when a crash occurs")]
    private ParticleSystem crashParticles;
    [SerializeField]
    [Tooltip("Distance of the particles away from the center of the car")]
    private float particleOffset = 2f;
    [SerializeField]
    [Tooltip("Audio source to play when a crash occurs")]
    private AudioSource crashAudio;
    [SerializeField]
    [Tooltip("List of objects that can detect collisions for the driver")]
    private CollisionEvents[] colliders;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        // Start base class
        base.Start();

        // Listen for all other colliders
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
            // Push myself away from the other driver
            PushAway(otherDriver);

            // Tell the other driver to push away from me
            otherDriver.crashingModule.PushAway(manager);
            otherDriver.DriverHitMeEvent.Invoke(manager);
        }
    }
    public void PushAway(DrivingManager otherDriver)
    {
        Vector3 toOther = otherDriver.rigidbody.position - manager.rigidbody.position;
        
        // Determine if the other driver is to the left or right of myself
        float leftOrRight = Vector3.Dot(toOther, manager.right);
        leftOrRight = Mathf.Sign(leftOrRight);

        // Get the direction that points at the other driver
        toOther = manager.right * leftOrRight;

        // Decompose the velocity components
        Vector3 rightComponent = toOther * crashSpeed * -1f;
        Vector3 upComponent = Vector3.Project(manager.rigidbody.velocity, manager.up);
        Vector3 forwardComponent = Vector3.Project(manager.rigidbody.velocity, manager.forward);

        // Set the velocity. This leaves forward and up unchanged while setting the right component
        manager.rigidbody.velocity = rightComponent + upComponent + forwardComponent;

        // Point the particles in the direction of shove and play the effect
        crashParticles.transform.forward = toOther;
        crashParticles.transform.localPosition = Vector3.right * leftOrRight * particleOffset;
        crashParticles.Play();

        // Play the crash audio and particle effect
        crashAudio.Play();
    }
    #endregion
}
