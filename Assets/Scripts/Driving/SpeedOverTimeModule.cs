using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedOverTimeModule : DrivingModule, ITopSpeedModifier
{
    #region Public Properties
    public float TimeSinceEffectStart => Time.time - timeOfEffectStart;
    public bool EffectActive => TimeSinceEffectStart < effectDuration;
    public float EffectInterpolator => TimeSinceEffectStart / effectDuration;
    public float MagnitudeInterpolator => curve.Evaluate(EffectInterpolator);
    public float CurrentEffectMagnitude => Mathf.Lerp(1f, effectMagnitude, MagnitudeInterpolator);
    public bool applyModifier => EffectActive;
    public float modifier => CurrentEffectMagnitude;
    public UnityEvent EffectStartEvent => effectStartEvent;
    public UnityEvent EffectUpdateEvent => effectUpdateEvent;
    public UnityEvent EffectStopEvent => effectStopEvent;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Modifier to apply to the top speed at the peak of the curve")]
    private float effectMagnitude = 1;
    [SerializeField]
    [Tooltip("Amount of time that the speed change is in effect")]
    private float effectDuration = 1;
    [SerializeField]
    [Tooltip("Animation curve that lerps between having no effect on the speed " +
        "(modifier = 1) and having full effect on the speed (modifier = effectMagnitude). " +
        "The curve should range between (0, 0) - (1, 1)")]
    private AnimationCurve curve;
    [SerializeField]
    [Tooltip("If this is true, the module applies a large force to the driver " +
        "to keep them at their top speed")]
    private bool applyForce;
    [SerializeField]
    [Tooltip("The strength of the force that keeps the driver at their top speed")]
    private float force = 100f;

    [Space]

    [SerializeField]
    [Tooltip("Event invoked when the effect starts")]
    private UnityEvent effectStartEvent;
    [SerializeField]
    [Tooltip("Event invoked for every update of the effect")]
    private UnityEvent effectUpdateEvent;
    [SerializeField]
    [Tooltip("Event invoked when the effect stops")]
    private UnityEvent effectStopEvent;
    #endregion

    #region Private Fields
    private float timeOfEffectStart = float.MinValue;
    private bool effectHasStopped = false;
    #endregion

    #region Monobehaviour Messages
    private void FixedUpdate()
    {
        // If the effect is active then invoke the event
        if (EffectActive)
        {
            // If we should apply a force then apply it
            if (applyForce)
            {
                manager.rigidbody.AddForce(manager.forward * force);
            }

            effectUpdateEvent.Invoke();
        }
        // If the effect is inactive but has not stopped
        // then stop the effect
        else if (!effectHasStopped)
        {
            StopEffect();
        }
    }
    #endregion

    #region Public Methods
    public bool StartEffectIfNotActive()
    {
        if (!EffectActive)
        {
            StartEffect();
            return true;
        }
        else return false;
    }
    public void StartEffect()
    {
        timeOfEffectStart = Time.time;
        effectHasStopped = false;
        effectStartEvent.Invoke();
    }
    public void StopEffect()
    {
        effectHasStopped = true;
        effectStopEvent.Invoke();
    }
    #endregion
}
