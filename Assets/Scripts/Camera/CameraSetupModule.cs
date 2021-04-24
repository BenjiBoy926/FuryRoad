using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetupModule : MonoBehaviour
{
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
            transform.position = GetGlobalPosition(backDistance);
        }
        transform.rotation = Quaternion.LookRotation(target.heading);
    }

    private void OnBoostUpdate(float boostPower)
    {
        boostUpdating = true;
        transform.position = GetGlobalPosition(backDistance + (maxBoostZoom * boostPower));
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
