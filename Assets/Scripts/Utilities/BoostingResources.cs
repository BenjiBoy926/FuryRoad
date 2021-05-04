using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoostingResources
{
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

    // Current boost power of the resources. When the power reaches 1,
    // increase boost resources by 1 and set the power back to 0
    private float boostPower;

    public int boostsAvailable => m_BoostsAvailable;
    public bool canBoost => boostsAvailable > 0;

    public void FixedUpdate(bool isDrifting, bool isDrafting, bool isAirborne)
    {
        if(isDrifting || isDrafting || isAirborne)
        {
            float rechargeRate = Mathf.Infinity;

            // Use the smallest recharge rate out of the applicable rates
            if (isDrifting) rechargeRate = Mathf.Min(rechargeRate, m_DriftRechargeRate);
            if (isDrafting) rechargeRate = Mathf.Min(rechargeRate, m_DraftRechargeRate);
            if (isAirborne) rechargeRate = Mathf.Min(rechargeRate, m_AirTimeRechargeRate);

            // Increase boost power at the smallest rate
            boostPower += Time.fixedDeltaTime / rechargeRate;

            // If boost power exceeds 1, then increase available boosts
            if(boostPower >= 1f)
            {
                m_BoostsAvailable++;
                boostPower = 0f;
            }
        }
        // While no action is taken to increase boost power, it slowly reduces to zero
        else
        {
            boostPower = Mathf.Max(0f, boostPower - (Time.fixedDeltaTime / m_BoostPowerGravity));
        }
    }

    public void ConsumeBoostResource()
    {
        m_BoostsAvailable--;
    }
}
