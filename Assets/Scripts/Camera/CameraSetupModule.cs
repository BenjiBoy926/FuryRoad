using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetupModule : MonoBehaviour
{
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

    // The target movement module
    private MovementModule3D target;
    private bool boostUpdating = false;

    public void Setup(Transform parent)
    {
        Camera existingCamera = parent.GetComponentInChildren<Camera>();

        // If no camera exists on the parent object yet, setup this camera for it
        if(existingCamera == null)
        {
            target = parent.GetComponent<MovementModule3D>();

            // Subscribe to boosting events on the movement module
            MovementModule3D movementModule = parent.GetComponent<MovementModule3D>();
            
            movementModule.boostingModule.onBoostUpdate.AddListener(OnBoostUpdate);
            movementModule.boostingModule.onBoostEnd.AddListener(OnBoostEnd);

            movementModule.driftingModule.driftBoost.onBoostUpdate.AddListener(OnBoostUpdate);
            movementModule.driftingModule.driftBoost.onBoostEnd.AddListener(OnBoostEnd);
        }
        // If a camera already exists for the parent object, destroy this camera
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if(!boostUpdating)
        {
            // Lerp towards the target position
            Vector3 target = GetGlobalPosition(backDistance);
            transform.position = Vector3.Lerp(transform.position, target, translateSpeed * Time.fixedDeltaTime);
        }
        // Lerp towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.heading), rotateSpeed * Time.fixedDeltaTime);
    }

    private void OnBoostUpdate(float boostPower)
    {
        boostUpdating = true;

        // Lerp towards a position that is further back from the car as the car boosts
        Vector3 target = GetGlobalPosition(backDistance + (maxBoostZoom * boostPower));
        transform.position = Vector3.Lerp(transform.position, target, translateSpeed * Time.fixedDeltaTime);
    }

    private void OnBoostEnd()
    {
        boostUpdating = false;
    }

    private Vector3 GetLocalPosition(float backDistance)
    {
        return -target.heading * backDistance + (target.groundingModule.groundNormal * lift);
    }
    private Vector3 GetGlobalPosition(float backDistance)
    {
        return target.rigidbody.position + GetLocalPosition(backDistance);
    }
}
