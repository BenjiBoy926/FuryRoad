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
    [SerializeField]
    [Tooltip("Reference to the renderer for the projectile")]
    private MeshRenderer mesh;
    [SerializeField]
    [Tooltip("Reference to the trail renderer for the projectile")]
    private TrailRenderer trail;
    #endregion

    #region Private Fields
    private bool destroyHasBroadcast = false;
    #endregion

    #region Monobehaviour Callbacks
    private void Awake()
    {
        // This does not seem to clear existing RPCs, resulting in errors when projectiles destroy each other
        projectile.destroySelf.SetOverride(DestroySelfOverNetwork);
    }
    private void Start()
    {
        // Get the car owned by this player
        GameObject owningCar = NetworkPlayer.GetCar(photonView.Owner);
        DrivingManager driver = owningCar.GetComponent<DrivingManager>();

        // Set the owner of the driver to this local driving manager
        if (driver)
        {
            projectile.SetOwner(driver);
        }
        else Debug.LogWarning($"No driving manager attached to the car " +
            $"owned by player {photonView.OwnerActorNr}");

        // Set the projectile's color
        if (photonView.IsMine) SetColor(Color.green);
        else SetColor(Color.red);
    }
    #endregion

    #region Private Methods
    private void DestroySelfOverNetwork()
    {
        // If this view is mine then destroy myself
        if (photonView.IsMine) DestroySelf();
        // If this photon view is not mine then notify the owner so they perform a network destroy
        else photonView.RPC(nameof(DestroySelf), photonView.Owner);
    }
    public void SetColor(Color color)
    {
        // Set the material's color
        mesh.material.color = color;

        // Set the trail start color
        trail.startColor = color;

        // Set the trail end color
        color.a = 0f;
        trail.endColor = color;
    }
    #endregion

    #region RPC Callbacks
    [PunRPC]
    public void DestroySelf()
    {
        if (!destroyHasBroadcast)
        {
            projectile.DestroyEffect();
            PhotonNetwork.Destroy(photonView);
            destroyHasBroadcast = true;
        }
    }
    [PunRPC]
    public void SelfHitReport(string message)
    {
        Debug.Log(message);
    }
    #endregion
}
