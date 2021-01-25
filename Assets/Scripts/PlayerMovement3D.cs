using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Movement information for the car")]
    private MovementModule3D movementModule;
    [TagSelector]
    [SerializeField]
    [Tooltip("Tag of the object that the car respawns at")]
    private string respawnTag;
    [SerializeField]
    [Tooltip("If the car passes below this height, it respawns on the track")]
    private float pitLevel;

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        movementModule.Turn(h);
        movementModule.Thrust(v);

        if(transform.position.y < pitLevel)
        {
            GameObject respawn = GameObject.FindGameObjectWithTag(respawnTag);

            if(respawn != null)
            {
                transform.position = respawn.transform.position;
                transform.rotation = respawn.transform.rotation;
            }
        }
    }
}
