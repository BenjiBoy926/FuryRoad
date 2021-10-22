using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The vehicle's top speed is changed by many different variables,
/// such as the type of terrain driving over 
/// and the boosting module active on the vehicle
/// </summary>
public class TopSpeedModule : DrivingModule
{
    #region Public Properties
    public float baseTopSpeed => m_BaseTopSpeed;
    public float currentTopSpeed
    {
        get; private set;
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("The base top speed of the vehicle")]
    private float m_BaseTopSpeed = 90f;
    #endregion

    #region Private Fields
    // List of the all the objects that can modify the top speed
    private ITopSpeedModifier[] topSpeedModifiers;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();

        // Get all top speed modifiers that are children of the driving manager
        topSpeedModifiers = m_Manager.gameObject.GetComponentsInChildren<ITopSpeedModifier>(true);
    }
    private void FixedUpdate()
    {
        float speed = m_BaseTopSpeed;

        // Multiply the modifiers of all applied modifiers
        foreach (ITopSpeedModifier modifier in topSpeedModifiers)
        {
            if (modifier.applyModifier) speed *= modifier.modifier;
        }

        // Set current top speed
        currentTopSpeed = speed;
    }
    #endregion
}
