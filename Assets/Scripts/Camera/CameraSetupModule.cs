using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetupModule : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Distance behind the car that the camera will sit")]
    private float backDistance;
    [SerializeField]
    [Tooltip("Distance above the car that the camera hovers")]
    private float lift;

    public void Setup(Transform parent)
    {
        Camera existingCamera = parent.GetComponentInChildren<Camera>();

        // If no camera exists on the parent object yet, setup this camera for it
        if(existingCamera == null)
        {
            transform.localRotation = Quaternion.LookRotation(parent.forward);
            transform.parent = parent;
            transform.localPosition = new Vector3(0f, lift, -backDistance);
        }
        // If a camera already exists for the parent object, destroy this camera
        else
        {
            Destroy(gameObject);
        }
    }
}
