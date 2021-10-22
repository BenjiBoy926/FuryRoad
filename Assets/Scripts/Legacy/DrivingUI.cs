using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DrivingUI : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the script that displays the speed of the player")]
    private DrivingSpeedUI speedUI;
    [SerializeField]
    [Tooltip("Reference to the script used to manage boost resources ui")]
    private BoostResourceUI boostResourceUI;
    [SerializeField]
    [Tooltip("Reference to the script used to manage placement of racers ui")]
    private PlacementUIManager placementUI;
    #endregion

    #region Public Methods
    public virtual void UpdatePlacementUI(int placement)
    {
        placementUI.UpdateUI(placement, PhotonNetwork.LocalPlayer);
    }
    #endregion
}
