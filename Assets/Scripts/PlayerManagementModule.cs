using System.Collections;
using UnityEngine;

public class PlayerManagementModule: MonoBehaviour
{
    private PlayerMovementDriver3D movementDriver;

    private void Awake()
    {
        movementDriver = GetComponent<PlayerMovementDriver3D>();
    }

    public void EnableControl(bool active)
    {
        movementDriver.enabled = active;
    }
}