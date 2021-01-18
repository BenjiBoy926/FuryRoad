using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Information used to move the cart")]
    private MovementModule movementModule;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movementModule.Turn(horizontal);
        movementModule.Thrust(vertical);
    }
}
