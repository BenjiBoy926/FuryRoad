using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class ResourceUI : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the prefab instantiated for each boosting resource")]
    private ResourceWidget resourceWidgetPrefab;
    [SerializeField]
    [Tooltip("Reference to the transform to use as the parent of all widgets")]
    private Transform resourceWidgetParent;
    [SerializeField]
    [Tooltip("Slider for the boost power")]
    [FormerlySerializedAs("powerSlider")]
    private Slider rechargeSlider;
    #endregion

    #region Private Fields
    private ResourceWidget[] resourceWidgets;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();

        // Create the array
        resourceWidgets = new ResourceWidget[manager.resources.ResourcesAvailable];
        
        // Instantiate a widget for each resource available at the start
        for(int i = 0; i < resourceWidgets.Length; i++)
        {
            resourceWidgets[i] = Instantiate(resourceWidgetPrefab, resourceWidgetParent);
        }

        // Add listener for when available boosts change
        manager.resources.onAvailableResourcesChanged.AddListener(OnAvailableResourcesChanged);
    }
    private void Update()
    {
        ResourcesModule resources = manager.resources;

        // Slider stays maxed out if all boosts are available
        if (resources.allResourcesAvailable)
        {
            rechargeSlider.value = rechargeSlider.maxValue;
        }
        // When not all boosts are available 
        // update the slider to match the resources recharge rate
        else
        {
            rechargeSlider.value = manager.resources.projectileRecharge;
        }
    }
    #endregion

    #region Private Methods
    private void OnAvailableResourcesChanged(int resources)
    {
        for(int i = 0; i < resourceWidgets.Length; i++)
        {
            resourceWidgets[i].SetActive(resources > i);
        }
    }
    #endregion
}
