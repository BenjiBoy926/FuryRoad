using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The vehicle's top speed is changed by many different variables,
/// such as the type of terrain driving over 
/// and the boosting module active on the vehicle
/// </summary>
[System.Serializable]
public class TopSpeedModule
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
    private List<ITopSpeedModifier> topSpeedModifiers = new List<ITopSpeedModifier>();
    #endregion

    #region Public Methods
    // Setup all objects that can modify the top speed
    public void Setup(params ITopSpeedModifier[] modifiers)
    {
        topSpeedModifiers.AddRange(modifiers);
        Debug.Log(string.Join("\n\t", topSpeedModifiers));
    }
    public void FixedUpdate()
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
