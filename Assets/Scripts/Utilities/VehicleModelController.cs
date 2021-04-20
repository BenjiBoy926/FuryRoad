using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleModelController : MonoBehaviour
{
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
        transform.forward = movementModule.heading;
    }
}
