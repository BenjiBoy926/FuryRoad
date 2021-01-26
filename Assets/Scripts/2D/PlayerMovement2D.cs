using UnityEngine;
using System.Collections;

public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Information used to move the car")]
    private MovementModule2D movementModule;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movementModule.Turn(horizontal);
        movementModule.Thrust(vertical);
    }
}
