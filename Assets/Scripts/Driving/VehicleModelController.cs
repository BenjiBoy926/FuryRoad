using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VehicleModelController : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the rigidbody on the model controller")]
    private Rigidbody rb;
    [SerializeField]
    [Tooltip("Amount that the vehicle model rotates away from the movement heading when drifting")]
    private float driftOffset;
    [SerializeField]
    [Tooltip("Speed that the model follows the heading of the car")]
    private float rotateSpeed;

    [Space]

    [SerializeField]
    [Tooltip("Amount that the wheels rotate while steering")]
    private float steerAngle = 45f;
    [SerializeField]
    [Tooltip("Offset of the wheel rotation while drifting")]
    private float driftSteerOffset = 45f;
    [SerializeField]
    [Tooltip("Amount that the wheels rotate while steering and drifting")]
    private float driftSteerAngle = 30f;
    [SerializeField]
    [Tooltip("List of wheels that steer the car")]
    private Transform[] steeringWheels;
    #endregion

    #region Monobehaviour Messages
    // Update is called once per frame
    void FixedUpdate()
    {
        // Copy the position of the movement module's rigidbody
        rb.position = manager.rigidbody.position;

        DriftingModule drifting = manager.driftingModule;
        Quaternion targetRotation;

        // If the drift is active, then do not look directly at the heading
        if (drifting.driftActive)
        {
            Quaternion rotation = Quaternion.Euler(0f, driftOffset * drifting.currentDirection, 0f);
            targetRotation = rotation * Quaternion.LookRotation(manager.forward);

            // Steer each of the wheels
            foreach(Transform wheel in steeringWheels)
            {
                RotateWheel(wheel, manager.steer, 
                    driftSteerOffset * manager.driftingModule.currentDirection * -1, 
                    driftSteerAngle);
            }
        }
        else
        {
            targetRotation = Quaternion.LookRotation(manager.forward);

            // Steer each of the wheels
            foreach(Transform wheel in steeringWheels)
            {
                RotateWheel(wheel, manager.steer, 0, steerAngle);
            }
        }

        // Rotate the forward vector towards the heading target
        rb.rotation = Quaternion.Lerp(rb.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
    }
    #endregion

    #region Private Methods
    private void RotateWheel(Transform wheel, float steer, float angleOffset, float maxAngle)
    {
        wheel.localRotation = Quaternion.Euler(0f, angleOffset + (steer * maxAngle), 0f);
    }
    #endregion
}
