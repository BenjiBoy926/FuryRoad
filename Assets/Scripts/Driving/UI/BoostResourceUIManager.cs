using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostResourceUIManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Script used to manage the collection of objects indicating the number of boosts available")]
    private BoostResourceCollection resourceCollection;
    [SerializeField]
    [Tooltip("Slider for the boost power")]
    private Slider powerSlider;

    public void UpdateUI(float power, int boosts)
    {
        powerSlider.value = power;
        resourceCollection.UpdateWidgets(boosts);
    }
}
