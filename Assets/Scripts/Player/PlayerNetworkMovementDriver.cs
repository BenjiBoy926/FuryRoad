using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworkMovementDriver : PlayerMovementDriver
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Photon view for this player")]
    private PhotonView view;
    #endregion

    #region Monobehaviour Messages
    protected override void Update()
    {
        // Do not setup input axes unless this photon view is mine
        // (You can also contorl the car if the network is not connected for debugging purposes)
        if (view.IsMine || !PhotonNetwork.IsConnected)
        {
            base.Update();
        }
    }
    protected override void FixedUpdate()
    {
        // Use the movement module to move the car
        // (You can also move the car if the network is not connected for debugging purposes)
        if (view.IsMine || !PhotonNetwork.IsConnected)
        {
            base.FixedUpdate();
        }
    }
    #endregion
}
