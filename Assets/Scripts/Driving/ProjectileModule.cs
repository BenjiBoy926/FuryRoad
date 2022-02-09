using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [Tooltip("Audio source used to play the launch sound")]
    private AudioSource source;
    [SerializeField]
    [Tooltip("List of sounds that could play when the projectile is fired")]
    private AudioClip[] launchSounds;
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
    public void TryFire(float dir)
    {
        if(!manager.boostingModule.EffectActive && manager.resources.hasResources)
        {
            Fire(dir);
        }
    }
    public void Fire(float dir)
    {
        // Compute projectile position
        Vector3 position = ComputeProjectilePosition(dir);
        // Create the projectile
        Projectile projectile = NetworkUtilities.InstantiateLocalOrNetwork(projectilePrefab, position, Quaternion.identity);
        // Setup the projectile
        projectile.Launch(manager, ComputeProjectileVelocity(dir));
        // Consume a resource once the projectile is fired
        manager.resources.ConsumeResource();

        // Play a launch sound
        PlaySound();
    }
    #endregion

    #region Protected Methods
    protected Vector3 ComputeProjectilePosition(float dir)
    {
        dir = Mathf.Sign(dir);
        return m_Manager.rigidbody.position + (m_Manager.forward * offset * dir);
    }
    protected Vector3 ComputeProjectileVelocity(float dir)
    {
        dir = Mathf.Sign(dir);
        return m_Manager.forward * dir * speed;
    }
    protected void PlaySound()
    {
        int index = Random.Range(0, launchSounds.Length);
        source.clip = launchSounds[index];
        source.Play();
    }
    #endregion
}
