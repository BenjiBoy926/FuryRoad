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
    [Tooltip("Event invoked when a projectile fired from this module hits a driver")]
    private DrivingManagerEvent driverHitEvent;
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
        if(!manager.boostingModule.boostActive && manager.boostResources.canBoost)
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
        projectile.Setup(manager, ComputeProjectileVelocity(dir));
        // Consume a boost resource once the projectile is fired
        manager.boostResources.ConsumeBoostResource();
    }
    #endregion

    #region Protected Methods
    protected Vector3 ComputeProjectilePosition(float dir)
    {
        dir = Mathf.Sign(dir);
        return m_Manager.rigidbody.position + (m_Manager.heading * offset * dir);
    }
    protected Vector3 ComputeProjectileVelocity(float dir)
    {
        dir = Mathf.Sign(dir);
        return m_Manager.heading * dir * speed;
    }
    #endregion
}
