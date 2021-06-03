using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the script that displays the speed of the player")]
    private SpeedUIManager speedUI;
    [SerializeField]
    [Tooltip("Reference to the script used to manage boost resources ui")]
    private BoostResourceUIManager boostResourceUI;
    [SerializeField]
    [Tooltip("Reference to the script used to manage placement of racers ui")]
    private PlacementUIManager placementUI;

    // Reference to the photon view of the car
    private PhotonView view;

    private void Awake()
    {
        // Get the view in the parent and disable gui if this is not my photon view
        view = GetComponentInParent<PhotonView>();
        gameObject.SetActive(view.IsMine);
    }

    public void UpdateBoostResourceUI(float boostPower, int boosts)
    {
        if(view.IsMine)
        {
            boostResourceUI.UpdateUI(boostPower, boosts);
        }
    }

    public void UpdateSpeedUI(float speed)
    {
        if(view.IsMine)
        {
            speedUI.UpdateUI(Mathf.Round(speed));
        }
    }

    public void UpdatePlacementUI(int placement)
    {
        if(view.IsMine)
        {
            placementUI.UpdateUI(placement);
        }
    }
}
