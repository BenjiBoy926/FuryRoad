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

    private Vector3 localPositionBase => new Vector3(0f, lift, -backDistance);

    public void Setup(Transform parent)
    {
        Camera existingCamera = parent.GetComponentInChildren<Camera>();

        // If no camera exists on the parent object yet, setup this camera for it
        if(existingCamera == null)
        {
            transform.localRotation = Quaternion.LookRotation(parent.forward);
            transform.parent = parent;
            transform.localPosition = localPositionBase;

            // Subscribe to boosting events on the movement module
            MovementModule3D movementModule = parent.GetComponent<MovementModule3D>();
            movementModule.boostingModule.onBoostUpdate.AddListener(OnBoostUpdate);
            movementModule.boostingModule.onBoostEnd.AddListener(OnBoostEnd);
        }
        // If a camera already exists for the parent object, destroy this camera
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnBoostUpdate(float boostPower)
    {
        transform.localPosition = localPositionBase + Vector3.ClampMagnitude(localPositionBase, boostPower * maxBoostZoom);
    }

    private void OnBoostEnd()
    {
        transform.localPosition = localPositionBase;
    }
}
