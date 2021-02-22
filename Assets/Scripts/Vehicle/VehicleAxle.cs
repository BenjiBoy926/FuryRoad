using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VehicleAxle
{
    [Tooltip("Wheel on the left of the axle")]
    public WheelCollider left;
    [Tooltip("Wheel on the right of the axle")]
    public WheelCollider right;

    public void Motor(float m)
    {
        left.motorTorque = m;
        right.motorTorque = m;
    }

    public void Brake(float b)
    {
        left.brakeTorque = b;
        right.brakeTorque = b;
    }

    public void Steer(float s)
    {
        left.steerAngle = s;
        right.steerAngle = s;
    }
}
