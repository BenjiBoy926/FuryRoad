using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementModule
{
    [SerializeField]
    [Tooltip("The rigidbody to move")]
    private Rigidbody2D rb2D;
    [SerializeField]
    [Tooltip("Power of the racer's acceleration")]
    private float thrust;
    [SerializeField]
    [Tooltip("Tightness of the racer's turn")]
    private float turn;
    [SerializeField]
    [Tooltip("Maximum speed of the racer")]
    private float topSpeed;

    public MovementModule(Rigidbody2D rb2D, float thrust, float turn)
    {
        this.rb2D = rb2D;
        this.thrust = thrust;
        this.turn = turn;
    }

    public void Turn(float horizontal)
    {
        rb2D.MoveRotation(rb2D.rotation - horizontal * turn);
    }

    public void Thrust(float vertical)
    {
        rb2D.AddRelativeForce(new Vector2(0, vertical * thrust));
        rb2D.velocity = Vector2.ClampMagnitude(rb2D.velocity, topSpeed);
    }
}
