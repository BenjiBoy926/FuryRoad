using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAimUI : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the player manager that aims the projectile")]
    private PlayerDriving player;
    [SerializeField]
    [Tooltip("Canvas group to fade out all the elements of the ui " +
        "when there are no projectiles to fire")]
    private CanvasGroup canvasGroup;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();
        manager.resources.onAvailableResourcesChanged.AddListener(UpdateCanvasGroupAlpha);
        UpdateCanvasGroupAlpha(manager.resources.ResourcesAvailable);
    }
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
        }
    }
    #endregion

    #region Private Methods
    private void UpdateCanvasGroupAlpha(int currentResources)
    {
        if (currentResources > 0) canvasGroup.alpha = 1;
        else canvasGroup.alpha = 0.3f;
    }
    #endregion
}
