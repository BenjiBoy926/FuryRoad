using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VehicleModelController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Amount that the vehicle model rotates away from the movement heading when drifting")]
    private float driftOffset;
    [SerializeField]
    [Tooltip("Speed that the model follows the heading of the car")]
    private float rotateSpeed;

    // Movement module on the parent
    private MovementModule3D movementModule;

    // Start is called before the first frame update
    void Start()
    {
        movementModule = GetComponentInParent<MovementModule3D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Copy the position of the movement module's rigidbody
        transform.position = movementModule.rigidbody.position;

        DriftingModule drifting = movementModule.driftingModule;
        Vector3 headingTarget;

        // If the drift is active, then 
        if (drifting.driftActive)
        {
            Quaternion rotation = Quaternion.Euler(0f, driftOffset * drifting.currentDirection, 0f);
            headingTarget = rotation * movementModule.heading;
        }
        else
        {
            headingTarget = movementModule.heading;
        }

        // Rotate the forward vector towards the heading target
        transform.forward = Vector3.RotateTowards(transform.forward, headingTarget, rotateSpeed * Time.fixedDeltaTime, 1000f);
    }
}
