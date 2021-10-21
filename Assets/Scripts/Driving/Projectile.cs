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
    private PlayerManager owner;
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
    public void Setup(PlayerManager owner, Vector3 velocity)
    {
        this.owner = owner;
        rb.velocity = velocity;
    }
    #endregion

    #region Private Methods
    private void HandleCollisionEnter(Collision collision)
    {
        HandlePlayerHit(collision.gameObject.GetComponentInParent<PlayerManager>());
    }
    private void HandleTriggerEnter(Collider other)
    {
        HandlePlayerHit(other.GetComponentInParent<PlayerManager>());
    }
    private void HandlePlayerHit(PlayerManager player)
    {
        if(player)
        {
            if (player == owner)
            {
                // Slow the owner a little
            }
            // If the other is not the owner then make the owner boost
            else owner.movementDriver.movementModule.StartBoost();

            // Destroy self when I hit a player
            NetworkUtilities.DestroyLocalOrNetwork(root);
        }
    } 
    #endregion
}
