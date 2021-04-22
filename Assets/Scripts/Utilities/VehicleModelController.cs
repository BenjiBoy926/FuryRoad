using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleModelController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Amount that the vehicle model rotates away from the movement heading when drifting")]
    private float driftOffset;

    // Movement module on the parent
    private MovementModule3D movementModule;

    // Start is called before the first frame update
    void Start()
    {
        movementModule = GetComponentInParent<MovementModule3D>();
    }

    // Update is called once per frame
    void Update()
    {
        DriftingModule drifting = movementModule.driftingModule;
        // If the drift is active, then 
        if(drifting.driftActive)
        {
            Quaternion rotation = Quaternion.Euler(0f, driftOffset * drifting.currentDirection, 0f);
            transform.forward = rotation * movementModule.heading;
        }
        else
        {
            transform.forward = movementModule.heading;
        }
    }
}
