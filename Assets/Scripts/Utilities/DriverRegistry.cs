using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Registers all of the drivers in the current scene in a list
/// </summary>
public static class DriverRegistry
{
    #region Public Properties
    public static IReadOnlyList<DrivingManager> Registry => registry;
    #endregion

    #region Private Fields
    private static List<DrivingManager> registry = new List<DrivingManager>();
    #endregion

    #region Public Methods
    public static void Register(DrivingManager newDriver)
    {
        // Add the driver to the registry
        registry.Add(newDriver);

        // Invoke the new driver entered event for every driver that has already registered
        foreach(DrivingManager driver in registry)
        {
            if (driver != newDriver)
            {
                driver.DriverRegisteredEvent.Invoke(newDriver);
            }
        }
    }
    public static void Deregister(DrivingManager removedDriver)
    {
        // Remove the driver
        bool existed = registry.Remove(removedDriver);

        // If the remove driver was in the list, then 
        // inform remaining drivers that a driver was removed
        if (existed)
        {
            foreach (DrivingManager driver in registry)
            {
                driver.DriverDeregisteredEvent.Invoke(driver);
            }
        }
    }
    #endregion
}
