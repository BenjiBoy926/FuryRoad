using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the rigidbody of the projectile")]
    private Rigidbody rb;
    [SerializeField]
    [Tooltip("Reference to the collision event hook on the physical part of the projectile")]
    private CollisionEventHook collisionEvents;
    [SerializeField]
    [Tooltip("Reference to the root object of the projectile")]
    private GameObject root;
    [SerializeField]
    [Tooltip("Transform of the object that renders a trail")]
    private Transform trail;
    [SerializeField]
    [Tooltip("Total lifetime of the projectile")]
    private float lifetime = 10f;
    #endregion

    #region Private Fields
    // Reference to the player who fired this projectile
    private DrivingManager owningDriver;
    // The time at which the projectile was created
    private float timeOfCreation;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        collisionEvents.CollisionEnter.AddListener(HandleCollisionEnter);
        collisionEvents.TriggerEnter.AddListener(HandleTriggerEnter);
        timeOfCreation = Time.time;
    }
    private void Update()
    {
        trail.position = rb.position;

        // Destoy myself if lifetime is up
        if(Time.time - timeOfCreation > lifetime)
        {
            NetworkUtilities.DestroyLocalOrNetwork(gameObject);
        }
    }
    #endregion

    #region Public Methods
    public void Setup(DrivingManager owningDriver, Vector3 velocity)
    {
        this.owningDriver = owningDriver;
        rb.velocity = velocity;
    }
    #endregion

    #region Private Methods
    private void HandleCollisionEnter(Collision collision)
    {
        HandlePlayerHit(collision.gameObject.GetComponentInParent<DrivingManager>());
    }
    private void HandleTriggerEnter(Collider other)
    {
        HandlePlayerHit(other.GetComponentInParent<DrivingManager>());
    }
    private void HandlePlayerHit(DrivingManager driver)
    {
        if(driver)
        {
            if (driver == owningDriver)
            {
                // Slow the owner a little
            }
            // If the other is not the owner then make the owner boost
            else owningDriver.boostingModule.StartBoosting();

            // Destroy self when I hit a player
            NetworkUtilities.DestroyLocalOrNetwork(root);
        }
    } 
    #endregion
}
