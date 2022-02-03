using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesModule : DrivingModule
{
    #region Private Typedefs
    [System.Serializable]
    private class IntEvent : UnityEvent<int> { }
    #endregion

    #region Public Propertes
    public int ResourcesAvailable => m_ResourcesAvailable;
    public bool allResourcesAvailable => m_ResourcesAvailable >= maxResources;
    public bool hasResources => ResourcesAvailable > 0;
    public float projectileRecharge => m_ChargeLevel;
    public UnityEvent<int> onAvailableResourcesChanged => m_OnAvailableResourcesChanged;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Number of boosts available to the vehicle")]
    private int m_ResourcesAvailable = 3;
    [SerializeField]
    [Tooltip("Number of seconds it takes to charge a boost while drifting")]
    private float m_RechargeRate = 2f;
    [SerializeField]
    [Tooltip("Event invoked when the number of boosts available changes")]
    private IntEvent m_OnAvailableResourcesChanged;
    #endregion

    #region Private Fields
    // Set to the initial value
    private int maxResources;
    // Current boost power of the resources. When the power reaches 1,
    // increase boost resources by 1 and set the power back to 0
    private float m_ChargeLevel;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();
        maxResources = m_ResourcesAvailable;
    }
    private void FixedUpdate()
    {
        // If all charges are available then cannot recharge right now
        if (allResourcesAvailable)
        {
            m_ChargeLevel = 0f;
        }
        else
        {
            // Increase charge level
            m_ChargeLevel += Time.fixedDeltaTime / m_RechargeRate;

            // If boost power exceeds 1, then increase available boosts
            if (m_ChargeLevel >= 1f)
            {
                SetResourcesAvailable(m_ResourcesAvailable + 1);
                m_ChargeLevel = 0f;
            }
        }
    }
    #endregion

    #region Public Methods
    public void ConsumeResource()
    {
        SetResourcesAvailable(m_ResourcesAvailable - 1);
    }
    public void SetResourcesAvailable(int resources)
    {
        // Store boosts before set
        int prevBoosts = m_ResourcesAvailable;
        // Set boosts available, clampled within min/max
        m_ResourcesAvailable = Mathf.Clamp(resources, 0, maxResources);

        // Anytime resources are set then reset the charge level
        m_ChargeLevel = 0f;

        // If previous is different from now then invoke the changed event
        if (prevBoosts != m_ResourcesAvailable) m_OnAvailableResourcesChanged.Invoke(m_ResourcesAvailable);
    }
    #endregion
}
