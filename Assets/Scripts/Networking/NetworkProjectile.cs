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

        // Override the projectile color
        projectile.color.SetOverride(GetColor);
    }
    private void Start()
    {
        GameObject owningCar = NetworkPlayer.GetCar(photonView.Owner);

        // Check if there is an owning car
        if (owningCar)
        {
            DrivingManager driver = owningCar.GetComponent<DrivingManager>();

            // Set the owner of the driver to this local driving manager
            if (driver)
            {
                projectile.SetOwner(driver);
            }
            else Debug.LogWarning($"No driving manager attached to the car " +
                $"owned by player {photonView.OwnerActorNr}");
        }
        else Debug.LogWarning($"Network projectile could not find a car " +
            $"owned by player {photonView.OwnerActorNr}, who owns this projectile");
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
    private Color GetColor()
    {
        if (photonView.IsMine) return Color.green;
        else return Color.red;
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
