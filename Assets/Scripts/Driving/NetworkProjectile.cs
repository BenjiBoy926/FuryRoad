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
    private void Start()
    {
        projectile.PrepareToDestroyEvent.AddListener(DestroySelfRPCSend);
    }
    #endregion

    #region RPC Callbacks
    private void DestroySelfRPCSend()
    {
        if(photonView.IsMine)
        {
            photonView.RPC(nameof(DestroySelfRPCReceive), RpcTarget.Others);
        }
    }
    [PunRPC]
    public void DestroySelfRPCReceive()
    {
        projectile.DestroySelf();
    }
    #endregion
}
