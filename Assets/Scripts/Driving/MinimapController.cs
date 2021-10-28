using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the transform of the camera that renders the minimap")]
    private Transform minimapCameraRoot;
    #endregion

    #region Private Fields
    // Offset of the camera, set at the start of the behaviour
    private Vector3 offset;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();

        // Set the camera's offset from the driving rigidbody
        offset = minimapCameraRoot.position - manager.rigidbody.position;
    }
    private void Update()
    {
        // Set the position of the minimap camera on each update
        minimapCameraRoot.position = manager.rigidbody.position + offset;

        // Set the up of the camera to point in the direction the vehicle is facing
        minimapCameraRoot.forward = manager.heading;
    }
    #endregion
}
