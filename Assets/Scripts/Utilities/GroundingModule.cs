using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundingModule : MonoBehaviour
{
    [SerializeField]
    [Tooltip("List of wheels on this vehicle")]
    private List<WheelCollider> wheels;

    public bool Grounded()
    {
        return wheels.TrueForAll(x => x.isGrounded);
    }
}