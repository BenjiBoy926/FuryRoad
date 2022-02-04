using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Speed at which the camera moves to follow the player")]
    private float translateSpeed = 20f;
    [SerializeField]
    [Tooltip("Speed at which the camera rotates to look at the player")]
    private float rotateSpeed = 20f;
    [SerializeField]
    [Tooltip("Distance behind the car that the camera will sit")]
    private float backDistance;
    [SerializeField]
    [Tooltip("Amount that the camera moves away from the car as the boost goes faster")]
    private float maxBoostZoom;
    [SerializeField]
    [Tooltip("Distance above the car that the camera hovers")]
    private float lift;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();

        manager.boostingModule.EffectUpdateEvent.AddListener(OnMainBoostUpdate);
        manager.driftingModule.driftBoost.EffectUpdateEvent.AddListener(OnDriftBoostUpdate);
    }

    private void FixedUpdate()
    {
        if(!manager.boostingModule.EffectActive)
        {
            // Lerp towards the target position
            Vector3 target = GetGlobalPosition(backDistance);
            transform.position = Vector3.Lerp(transform.position, target, translateSpeed * Time.fixedDeltaTime);
        }
        // Lerp towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(manager.forward), rotateSpeed * Time.fixedDeltaTime);
    }
    #endregion

    #region Private Methods
    private void OnMainBoostUpdate()
    {
        OnBoostUpdate(manager.boostingModule.MagnitudeInterpolator);
    }
    // Only update the position for a drift boost if the main boost is inactive
    private void OnDriftBoostUpdate()
    {
        if (!manager.boostingModule.EffectActive)
        {
            OnBoostUpdate(manager.driftingModule.driftBoost.MagnitudeInterpolator);
        }
    }
    private void OnBoostUpdate(float boostPower)
    {
        // Lerp towards a position that is further back from the car as the car boosts
        Vector3 target = GetGlobalPosition(backDistance + (maxBoostZoom * boostPower));
        transform.position = Vector3.Lerp(transform.position, target, translateSpeed * Time.fixedDeltaTime);
    }
    private Vector3 GetLocalPosition(float backDistance)
    {
        return (-manager.forward * backDistance) + (manager.groundingModule.groundNormal * lift);
    }
    private Vector3 GetGlobalPosition(float backDistance)
    {
        return manager.rigidbody.position + GetLocalPosition(backDistance);
    }
    #endregion
}
