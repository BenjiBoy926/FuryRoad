using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementModule3D
{
    [SerializeField]
    [Tooltip("The rigidbody to move")]
    private Rigidbody rb;
    [SerializeField]
    [Tooltip("Power of the racer's acceleration")]
    private float thrust = 10f;
    [SerializeField]
    [Tooltip("Tightness of the racer's turn")]
    private float turn = 10f;
    [SerializeField]
    [Tooltip("Maximum speed of the racer")]
    private float topSpeed = 30f;

    public MovementModule3D(Rigidbody rb, float thrust, float turn)
    {
        this.rb = rb;
        this.thrust = thrust;
        this.turn = turn;
    }

    public void Turn(float horizontal)
    {
        Quaternion rotation = Quaternion.Euler(0f, horizontal * turn, 0f);
        rb.MoveRotation(rb.rotation * rotation);
        rb.velocity = rotation * rb.velocity;
    }

    public void Thrust(float vertical)
    {
        rb.AddRelativeForce(Vector3.forward * vertical * thrust);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, topSpeed);
    }
}
