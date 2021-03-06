﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainModule : ITopSpeedModifier
{
    [SerializeField]
    [Tooltip("Tag of the object considered to be 'offroad' terrain")]
    private string offroadTag = "Offroad";
    [SerializeField]
    [Tooltip("Top speed adjustment while the vehicle is driving offroad")]
    private float offroadModifier;

    // True if the vehicle is currently off the road
    private bool isOffroad;

    public float modifier => offroadModifier;
    public bool applyModifier => isOffroad;

    public void FixedUpdate(GroundingModule groundingModule)
    {
        if(groundingModule.hit.collider != null)
        {
            isOffroad = groundingModule.hit.collider.CompareTag(offroadTag);
        }
    }
}
