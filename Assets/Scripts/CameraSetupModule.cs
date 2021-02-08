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
        transform.parent = parent;
        transform.localRotation = Quaternion.LookRotation(parent.forward);
        transform.localPosition = new Vector3(0f, lift, -backDistance);
    }
}
