using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostResourceUI : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the prefab instantiated for each boosting resource")]
    private BoostResourceWidget resourceWidgetPrefab;
    [SerializeField]
    [Tooltip("Reference to the transform to use as the parent of all widgets")]
    private Transform resourceWidgetParent;
    [SerializeField]
    [Tooltip("Slider for the boost power")]
    private Slider powerSlider;
    #endregion

    #region Private Fields
    private BoostResourceWidget[] resourceWidgets;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();

        // Create the array
        resourceWidgets = new BoostResourceWidget[manager.boostResources.boostsAvailable];
        
        // Instantiate a widget for each resource available at the start
        for(int i = 0; i < resourceWidgets.Length; i++)
        {
            resourceWidgets[i] = Instantiate(resourceWidgetPrefab, resourceWidgetParent);
        }

        // Add listener for when available boosts change
        manager.boostResources.onAvailableBoostsChanged.AddListener(OnAvailableBoostsChanged);
    }
    private void Update()
    {
        BoostingResources resources = manager.boostResources;

        // Slider stays maxed out if all boosts are available
        if (resources.allBoostsAvailable)
        {
            powerSlider.value = powerSlider.maxValue;
        }
        // When not all boosts are available 
        // update the slider to match the resources recharge rate
        else
        {
            powerSlider.value = manager.boostResources.boostRecharge;
        }
    }
    #endregion

    #region Private Methods
    private void OnAvailableBoostsChanged(int availableBoosts)
    {
        for(int i = 0; i < resourceWidgets.Length; i++)
        {
            resourceWidgets[i].SetActive(availableBoosts > i);
        }
    }
    #endregion
}
