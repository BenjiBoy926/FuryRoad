using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementModule : MonoBehaviour
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

    public MovementModule(Rigidbody2D rb2D, float thrust, float turn)
    {
        this.rb2D = rb2D;
        this.thrust = thrust;
        this.turn = turn;
    }

    public void Turn(float horizontal)
    {
        rb2D.MoveRotation(horizontal * turn * Time.fixedDeltaTime);
    }

    public void Thrust(float vertical)
    {
        rb2D.AddRelativeForce(new Vector2(0, vertical * thrust) * Time.fixedDeltaTime);
    }
}
