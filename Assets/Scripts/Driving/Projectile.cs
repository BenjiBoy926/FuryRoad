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
    private CollisionEvents sphereCollisionEvents;
    [SerializeField]
    [Tooltip("Reference to the collision events on the outer, wider trigger of the projectile")]
    private CollisionEvents triggerCollisionEvents;
    [SerializeField]
    [Tooltip("Reference to the root object of the projectile")]
    private GameObject root;
    [SerializeField]
    [Tooltip("Total lifetime of the projectile")]
    private float lifetime = 10f;
    [SerializeField]
    [Tooltip("List of objects that will follow the position of the rigidbody programmatically")]
    private Transform[] followingObjects;
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
        // Handle collision/trigger for the sphere by checking if the player was hit
        sphereCollisionEvents.CollisionEnter.AddListener(BallCollisionEnter);
        sphereCollisionEvents.TriggerEnter.AddListener(BallTriggerEnter);

        // Handle event when the extended trigger enters something
        triggerCollisionEvents.TriggerEnter.AddListener(ExtendedTriggerEnter);

        timeOfCreation = Time.time;
    }
    private void Update()
    {
        // Update the position of each following object
        foreach(Transform follower in followingObjects)
        {
            follower.position = rb.position;
        }

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
    private void BallCollisionEnter(Collision collision)
    {
        HandlePlayerPhysicsEnter(collision.gameObject.GetComponentInParent<DrivingManager>());
    }
    private void BallTriggerEnter(Collider other)
    {
        HandlePlayerPhysicsEnter(other.GetComponentInParent<DrivingManager>());
    }
    private void HandlePlayerPhysicsEnter(DrivingManager driver)
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
            DestroySelf();
        }
    }
    
    // Action taken when the extended trigger on the projectile enters some other object
    private void ExtendedTriggerEnter(Collider other)
    {
        Projectile otherProjectile = other.GetComponentInParent<Projectile>();

        // If another projectile was found that is not this projectile then the projectiles destroy each other
        if(otherProjectile && otherProjectile != this)
        {
            otherProjectile.DestroySelf();
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        NetworkUtilities.DestroyLocalOrNetwork(root);
    }
    #endregion
}
