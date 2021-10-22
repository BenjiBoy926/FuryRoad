using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoostingResources : DrivingModule
{
    #region Private Typedefs
    [System.Serializable]
    private class IntEvent : UnityEvent<int> { }
    #endregion

    #region Public Propertes
    public int boostsAvailable => m_BoostsAvailable;
    public bool canBoost => boostsAvailable > 0;
    public float boostRecharge => m_BoostRecharge;
    public UnityEvent<int> onAvailableBoostsChanged => m_OnAvailableBoostsChanged;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Script used to manage player UI")]
    private DrivingUI ui;
    [SerializeField]
    [Tooltip("Number of boosts available to the vehicle")]
    private int m_BoostsAvailable = 3;
    [SerializeField]
    [Tooltip("Number of seconds it takes to charge a boost while drifting")]
    private float m_DriftRechargeRate = 2f;
    [SerializeField]
    [Tooltip("Number of seconds it takes to charge a boost while drafting")]
    private float m_DraftRechargeRate = 1.5f;
    [SerializeField]
    [Tooltip("Number of seconds it takes to charge a boost while in the air")]
    private float m_AirTimeRechargeRate = 1f;
    [SerializeField]
    [Tooltip("Seconds it takes for boost power built up to fall back down to zero")]
    private float m_BoostPowerGravity = 3f;
    [SerializeField]
    [Tooltip("Event invoked when the number of boosts available changes")]
    private IntEvent m_OnAvailableBoostsChanged;
    #endregion

    #region Private Fields
    // Set to the initial value
    private int maxBoosts;
    // Current boost power of the resources. When the power reaches 1,
    // increase boost resources by 1 and set the power back to 0
    private float m_BoostRecharge;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();
        maxBoosts = m_BoostsAvailable;
    }
    private void FixedUpdate()
    {
        // Get some helpful bools
        bool isDrifting = m_Manager.driftingModule.driftActive;
        bool isDrafting = m_Manager.draftingModule.draftActive;
        bool isAirborne = !m_Manager.groundingModule.grounded;

        if(isDrifting || isDrafting || isAirborne)
        {
            float rechargeRate = Mathf.Infinity;

            // Use the smallest recharge rate out of the applicable rates
            if (isDrifting) rechargeRate = Mathf.Min(rechargeRate, m_DriftRechargeRate);
            if (isDrafting) rechargeRate = Mathf.Min(rechargeRate, m_DraftRechargeRate);
            if (isAirborne) rechargeRate = Mathf.Min(rechargeRate, m_AirTimeRechargeRate);

            // Increase boost power at the smallest rate
            m_BoostRecharge += Time.fixedDeltaTime / rechargeRate;

            // If boost power exceeds 1, then increase available boosts
            if(m_BoostRecharge >= 1f)
            {
                SetBoostsAvailable(m_BoostsAvailable + 1);
                m_BoostRecharge = 0f;
            }
        }
        // While no action is taken to increase boost power, it slowly reduces to zero
        else
        {
            m_BoostRecharge = Mathf.Max(0f, m_BoostRecharge - (Time.fixedDeltaTime / m_BoostPowerGravity));
        }
    }
    #endregion

    #region Public Methods
    public void ConsumeBoostResource()
    {
        SetBoostsAvailable(m_BoostsAvailable - 1);
    }
    public void SetBoostsAvailable(int boosts)
    {
        // Store boosts before set
        int prevBoosts = m_BoostsAvailable;
        // Set boosts available, clampled within min/max
        m_BoostsAvailable = Mathf.Clamp(boosts, 0, maxBoosts);

        // If previous is different from now then invoke the changed event
        if (prevBoosts != m_BoostsAvailable) m_OnAvailableBoostsChanged.Invoke(m_BoostsAvailable);
    }
    #endregion
}
