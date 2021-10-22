using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all modules used to drive a car
/// Each module references the manager and only references
/// submodules through the manager
/// </summary>
public class DrivingModule : MonoBehaviour
{
    #region Protected Fields
    protected DrivingManager manager;
    #endregion

    #region Monobehaviour Messages
    protected virtual void Start()
    {
        manager = GetComponentInParent<DrivingManager>();

        if (!manager) throw new MissingComponentException($"{GetType().Name}: " +
             $"game object named '{name}' " +
             $"expected to find a component of type '{nameof(DrivingManager)}' " +
             $"attached to this game object or one of its parent game objects, " +
             $"but no such component could be found.  Make sure this driving module " +
             $"has a component of type '{nameof(DrivingManager)}' attached to this game object " +
             $"or one of the game object's parents");
    }
    #endregion
}
