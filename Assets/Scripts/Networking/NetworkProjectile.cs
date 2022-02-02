using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class NetworkProjectile : MonoBehaviourPunCallbacks
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the projectile to sync on the network")]
    private Projectile projectile;
    #endregion

    #region Monobehaviour Callbacks
    private void Awake()
    {
        // This does not seem to clear existing RPCs, resulting in errors when projectiles destroy each other
        projectile.destroySelf.SetOverride(DestroySelfOverNetwork);
    }
    #endregion

    #region Private Methods
    private void DestroySelfOverNetwork()
    {
        // If this view is mine then destroy myself
        if (photonView.IsMine) PhotonNetwork.Destroy(photonView);
        // If this photon view is not mine then notify the owner so they perform a network destroy
        else photonView.RPC(nameof(DestroySelfRPCReceive), photonView.Owner);
    }
    #endregion

    #region RPC Callbacks
    [PunRPC]
    public void DestroySelfRPCReceive()
    {
        PhotonNetwork.Destroy(photonView);
    }
    #endregion
}
