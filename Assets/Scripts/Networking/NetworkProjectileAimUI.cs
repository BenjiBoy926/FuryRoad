using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class NetworkProjectileAimUI : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the photon view " +
        "used to check if we are the local player")]
    private PhotonView photonView;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();

        Image[] images = GetComponentsInChildren<Image>();

        // Set image colors to green if they are mine 
        // and red if they belong to someone else
        foreach (Image image in images)
        {
            if (photonView.IsMine) image.color = Color.green;
            else image.color = Color.red;
        }
    }
    #endregion
}
