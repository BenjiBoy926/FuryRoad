using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VehicleModelController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the rigidbody on the model controller")]
    private Rigidbody rb;
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
        rb.position = movementModule.rigidbody.position;

        DriftingModule drifting = movementModule.driftingModule;
        Quaternion targetRotation;

        // If the drift is active, then 
        if (drifting.driftActive)
        {
            Quaternion rotation = Quaternion.Euler(0f, driftOffset * drifting.currentDirection, 0f);
            targetRotation = rotation * Quaternion.LookRotation(movementModule.heading);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(movementModule.heading);
        }

        // Rotate the forward vector towards the heading target
        rb.rotation = Quaternion.Lerp(rb.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
    }
}
