using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerUIManager : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the script that displays the speed of the player")]
    private SpeedUIManager speedUI;
    [SerializeField]
    [Tooltip("Reference to the script used to manage boost resources ui")]
    private BoostResourceUIManager boostResourceUI;
    [SerializeField]
    [Tooltip("Reference to the script used to manage placement of racers ui")]
    private PlacementUIManager placementUI;
    #endregion

    #region Public Methods
    public virtual void UpdateBoostResourceUI(float boostPower, int boosts)
    {
        boostResourceUI.UpdateUI(boostPower, boosts);
    }
    public virtual void UpdateSpeedUI(float speed)
    {
        speedUI.UpdateUI(speed);
    }
    public virtual void UpdatePlacementUI(int placement)
    {
        placementUI.UpdateUI(placement, PhotonNetwork.LocalPlayer);
    }
    #endregion
}
