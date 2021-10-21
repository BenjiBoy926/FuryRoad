using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileModule : MonoBehaviour
{
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
    #endregion

    #region Public Methods
    public virtual void Fire(PlayerManager owner, Vector3 origin, Vector3 forward, float dir)
    {
        // Compute projectile position
        Vector3 position = ComputeProjectilePosition(origin, forward, dir);
        // Create the projectile
        Projectile projectile = NetworkUtilities.InstantiateLocalOrNetwork(projectilePrefab, position, Quaternion.identity);
        // Setup the projectile
        projectile.Setup(owner, ComputeProjectileVelocity(forward, dir));
    }
    #endregion

    #region Protected Methods
    protected Vector3 ComputeProjectilePosition(Vector3 origin, Vector3 forward, float dir)
    {
        dir = Mathf.Sign(dir);
        return origin + (forward * offset * dir);
    }
    protected Vector3 ComputeProjectileVelocity(Vector3 forward, float dir)
    {
        dir = Mathf.Sign(dir);
        return forward.normalized * dir * speed;
    }
    #endregion
}
