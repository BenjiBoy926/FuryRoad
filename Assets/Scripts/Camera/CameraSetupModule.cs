﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetupModule : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Parent transform that should have all of the important components attached")]
    private Transform root;
    [SerializeField]
    [Tooltip("Speed at which the camera moves to follow the player")]
    private float translateSpeed = 20f;
    [SerializeField]
    [Tooltip("Speed at which the camera rotates to look at the player")]
    private float rotateSpeed = 20f;
    [SerializeField]
    [Tooltip("Distance behind the car that the camera will sit")]
    private float backDistance;
    [SerializeField]
    [Tooltip("Amount that the camera moves away from the car as the boost goes faster")]
    private float maxBoostZoom;
    [SerializeField]
    [Tooltip("Distance above the car that the camera hovers")]
    private float lift;

    // The target movement module
    private MovementManager target;
    private bool boostUpdating = false;

    private void Awake()
    {
        Setup(root);
    }

    public void Setup(Transform parent)
    {
        target = parent.GetComponent<MovementManager>();

        target.boostingModule.onBoostUpdate.AddListener(OnBoostUpdate);
        target.boostingModule.onBoostEnd.AddListener(OnBoostEnd);

        target.driftingModule.driftBoost.onBoostUpdate.AddListener(OnDriftBoostUpdate);
        target.driftingModule.driftBoost.onBoostEnd.AddListener(OnDriftBoostEnd);
    }

    private void FixedUpdate()
    {
        if(!boostUpdating)
        {
            // Lerp towards the target position
            Vector3 target = GetGlobalPosition(backDistance);
            transform.position = Vector3.Lerp(transform.position, target, translateSpeed * Time.fixedDeltaTime);
        }
        // Lerp towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.heading), rotateSpeed * Time.fixedDeltaTime);
    }

    private void OnBoostUpdate(float boostPower)
    {
        boostUpdating = true;

        // Lerp towards a position that is further back from the car as the car boosts
        Vector3 target = GetGlobalPosition(backDistance + (maxBoostZoom * boostPower));
        transform.position = Vector3.Lerp(transform.position, target, translateSpeed * Time.fixedDeltaTime);
    }

    private void OnBoostEnd()
    {
        boostUpdating = false;
    }

    // Only update the position for a drift boost if the main boost is inactive
    private void OnDriftBoostUpdate(float boostPower)
    {
        if (!target.boostingModule.boostActive) OnBoostUpdate(boostPower);
    }
    // Only end the boost if the main boost is not also active
    private void OnDriftBoostEnd()
    {
        if (!target.boostingModule.boostActive) OnBoostEnd();
    }

    private Vector3 GetLocalPosition(float backDistance)
    {
        return -target.heading * backDistance + (target.groundingModule.groundNormal * lift);
    }
    private Vector3 GetGlobalPosition(float backDistance)
    {
        return target.rigidbody.position + GetLocalPosition(backDistance);
    }
}
