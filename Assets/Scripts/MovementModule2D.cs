using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementModule2D
{
    [SerializeField]
    [Tooltip("The rigidbody to move")]
    private Rigidbody2D rb2D;
    [SerializeField]
    [Tooltip("Power of the racer's acceleration")]
    private float thrust = 10f;
    [SerializeField]
    [Tooltip("Tightness of the racer's turn")]
    private float turn = 10f;
    [SerializeField]
    [Tooltip("Maximum speed of the racer")]
    private float topSpeed = 30f;

    public MovementModule2D(Rigidbody2D rb2D, float thrust, float turn)
    {
        this.rb2D = rb2D;
        this.thrust = thrust;
        this.turn = turn;
    }

    public void Turn(float horizontal)
    {
        float angle = -(horizontal * turn);
        rb2D.MoveRotation(rb2D.rotation + angle);
        //rb2D.velocity = Quaternion.Euler(0f, 0f, angle) * rb2D.velocity;
    }

    public void Thrust(float vertical)
    {
        rb2D.AddRelativeForce(Vector2.up * vertical * thrust);
        rb2D.velocity = Vector2.ClampMagnitude(rb2D.velocity, topSpeed);
    }
}
