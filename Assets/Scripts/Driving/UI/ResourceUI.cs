using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using DG.Tweening;

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

    [SerializeField]
    [Tooltip("Audio source used to play UI sounds")]
    private AudioSource uiAudioSource;
    [SerializeField]
    [Tooltip("Audio that plays when a new resource is gained")]
    private AudioClip rechargeAudioClip;
    [SerializeField]
    [Tooltip("Amount that the scale increases when a new resource is gained")]
    private float scalePunch = 0.5f;
    [SerializeField]
    [Tooltip("Time of the scale punch when a new resource is gained")]
    private float scalePunchTime = 0.2f;
    #endregion

    #region Private Fields
    private ResourceWidget[] resourceWidgets;
    private int previousResources;
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

        // Setup the previous resources
        previousResources = manager.resources.ResourcesAvailable;

        // Update all on the start
        OnAvailableResourcesChanged(manager.resources.ResourcesAvailable);
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

        // If current resources are more than previous resources,
        // then call the method to create the effect for resources increased
        if (resources > previousResources) OnResourcesIncreased();

        // Set previous resources
        previousResources = resources;
    }
    private void OnResourcesIncreased()
    {
        // Punch the scale of the transform
        transform.DOPunchScale(Vector3.one * scalePunch, scalePunchTime);

        // Play the recharge clip
        uiAudioSource.clip = rechargeAudioClip;
        uiAudioSource.Play();
    }
    #endregion
}
