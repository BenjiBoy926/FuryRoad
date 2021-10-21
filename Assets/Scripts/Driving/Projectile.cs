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
    #endregion

    #region Private Fields
    // Reference to the player who fired this projectile
    private PlayerManager owner;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        collisionEvents.CollisionEnter.AddListener(HandleCollisionEnter);   
    }
    private void Update()
    {
        trail.position = rb.position;
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
        // Try to get a player manager in the parent of the object hit
        PlayerManager other = collision.gameObject.GetComponentInParent<PlayerManager>();

        if (other)
        {
            if (other == owner)
            {
                // Slow down the owner a little
            }
            // If the other is not the owner, then make the owner boost
            else owner.movementDriver.movementModule.StartBoost();

            // Destroy self when I hit a player
            Destroy(root);
        }
    }
    #endregion
}
