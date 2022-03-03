using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAimUI : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the player manager that aims the projectile")]
    private PlayerDriving player;
    #endregion

    #region Monobehaviour Messages
    private void Update()
    {        
        // Move this ui slightly above the bottom down point of the collider
        Bounds rigidbodyBounds = manager.rigidbodyCollider.bounds;
        transform.position = rigidbodyBounds.center
            + Vector3.down * rigidbodyBounds.extents.y
            + Vector3.up * 0.1f;

        // Transform the projectile direction into the driver's direction
        if (player.projectileAxis.sqrMagnitude > 0.1f)
        {
            Vector3 direction = new Vector3(player.projectileAxis.x, 0f, player.projectileAxis.y);
            direction = manager.TransformDirection(direction);
            transform.rotation = Quaternion.LookRotation(-manager.up, direction);
            Debug.Log(direction);
        }
    }
    #endregion
}
