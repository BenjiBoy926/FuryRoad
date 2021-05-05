using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the photon view on the root object")]
    private PhotonView view;
    [SerializeField]
    [Tooltip("Reference to the script used to manage boost resources ui")]
    private BoostResourceUIManager boostResourceUI;

    private void Awake()
    {
        // Disable gui if this is not my photon view
        gameObject.SetActive(view.IsMine);
    }

    public void UpdateBoostResourceUI(float boostPower, int boosts)
    {
        if(view.IsMine)
        {
            boostResourceUI.UpdateUI(boostPower, boosts);
        }
    }
}
