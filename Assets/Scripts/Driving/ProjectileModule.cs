using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ProjectileModule : DrivingModule
{
    #region Public Typedefs
    [System.Serializable]
    public class DrivingManagerEvent : UnityEvent<DrivingManager> { }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Projectile prefab to instantiate when firing a projectile")]
    private Projectile projectilePrefab;
    [SerializeField]
    [Tooltip("Speed to fire the projectile at")]
    private float speed = 100f;
    [SerializeField]
    [Tooltip("Offset from the player manager that the projectile is placed")]
    private float offset = 5f;
    [SerializeField]
    [Tooltip("Particle system to start up when the projectile is launched")]
    [FormerlySerializedAs("launchParticle")]
    private ParticleSystem launchParticle;
    [SerializeField]
    [Tooltip("Audio source used to play the launch sound")]
    private AudioSource launchSource;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Fire a projectile only if the driving is not boosting,
    /// and there are resources left
    /// </summary>
    /// <param name="dir"></param>
    public void TryFire(Vector2 dir)
    {
        if(!manager.boostingModule.EffectActive && manager.resources.hasResources)
        {
            Fire(dir);
        }
    }
    public void Fire(Vector2 dir)
    {
        // Compute projectile position and direction
        Vector3 position = ComputeProjectilePosition(dir);
        Vector3 direction = ComputeProjectileVelocity(dir);
        // Create the projectile
        Projectile projectile = NetworkUtilities.InstantiateLocalOrNetwork(projectilePrefab, position, Quaternion.identity);
        // Setup the projectile
        projectile.Launch(manager, direction);
        // Consume a resource once the projectile is fired
        manager.resources.ConsumeResource();

        // Setup and launch the particle
        launchParticle.transform.position = position;
        launchParticle.transform.forward = direction;
        launchParticle.Play();

        // Play a launch sound
        launchSource.Play();
    }
    #endregion

    #region Protected Methods
    protected Vector3 ComputeProjectilePosition(Vector2 dir)
    {
        Vector3 direction = new Vector3(dir.x, 0f, dir.y);
        direction = manager.TransformDirection(direction);
        return m_Manager.rigidbody.position + (direction * offset);
    }
    protected Vector3 ComputeProjectileVelocity(Vector2 dir)
    {
        Vector3 direction = new Vector3(dir.x, 0f, dir.y);
        direction = direction.normalized;
        direction = manager.TransformDirection(direction);
        return direction * speed;
    }
    #endregion
}
