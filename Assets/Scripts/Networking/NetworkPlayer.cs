using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Setup the player for networked play by enabling/disabling
/// game objects and components on the player based on whether
/// the given photon view is mine
/// </summary>
public class NetworkPlayer : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the script that lets the player drive using a driving manager")]
    private PlayerDriving player;
    [SerializeField]
    [Tooltip("Reference to the ui that displays when a projectile hits someone")]
    private ProjectileHitUI projectileUI;
    [SerializeField]
    [Tooltip("List of game objects to enable/disable depending on ownership of the photon view provided")]
    private GameObject[] networkSensitiveObjects;
    #endregion

    #region Monobehaviour Messages
    private void Awake()
    {
        player.setControl.SetOverride(enabled =>
        {
            // Use the virtual version of set control
            // only if this is the local player
            if (photonView.IsMine)
            {
                player.setControl.InvokeVirtual(enabled);
            }
        });

        // Over the network, the actor number is determined
        // by the number of the photon player who owns the driver
        player.drivingManager.driverNumber.SetOverride(() => photonView.OwnerActorNr);
    }
    private void Start()
    {
        // Listen for when my projectile hits another and when another hits me
        player.drivingManager.ProjectileHitOtherEvent.AddListener(OnProjectileHitOther);
        player.drivingManager.ProjectileHitMeEvent.AddListener(OnProjectileHitMe);

        // Disable the player driving if they are not owned by me
        player.setControl.InvokeVirtual(photonView.IsMine);

        // Setup network sensitive objects
        foreach(GameObject obj in networkSensitiveObjects)
        {
            obj.SetActive(photonView.IsMine);
        }
    }
    #endregion

    #region Event Listeners
    private void OnProjectileHitOther(DrivingManager other)
    {
        PhotonView otherView = other.GetComponent<PhotonView>();

        // If we got the other view, then send an rpc to the owner of this player
        // saying that our projectile hit the other
        if (otherView)
        {
            photonView.RPC(nameof(OnProjectileHitOtherRPC), photonView.Owner, otherView.OwnerActorNr);

            // Notify the other player that a projectile hit them
            otherView.RPC(nameof(OnProjectileHitMeRPC), otherView.Owner, photonView.OwnerActorNr);
        }
    }
    private void OnProjectileHitMe(Projectile projectile)
    {
        PhotonView projectileView = projectile.GetComponent<PhotonView>();

        // If we got a photon view on the projectile,
        // use it to create the projectile ui animation
        if (projectileView)
        {
            photonView.RPC(nameof(OnProjectileHitMeRPC), photonView.Owner, projectileView.OwnerActorNr);

            // Get the car (on our local machine) being controlled by the same player
            // who controls the projectile hit
            GameObject otherCar = GetCar(projectileView.Owner);
            PhotonView otherView = otherCar.GetComponent<PhotonView>();

            // Notify the other player that their projectile hit me
            if (otherView)
            {
                otherView.RPC(nameof(OnProjectileHitOtherRPC), otherView.Owner, photonView.OwnerActorNr);
            }
        }
    }
    #endregion

    #region Interface Implementation
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = gameObject;
    }
    #endregion

    #region RPC Targets
    [PunRPC]
    public void OnProjectileHitOtherRPC(int otherActorNumber)
    {
        GameObject otherCar = GetCar(otherActorNumber);
        DrivingManager otherDriver = otherCar.GetComponent<DrivingManager>();
        projectileUI.AnimateProjectileHitOther(otherDriver.ID);
    }
    [PunRPC]
    public void OnProjectileHitMeRPC(int projectileActorNumber)
    {
        GameObject otherCar = GetCar(projectileActorNumber);
        DrivingManager otherDriver = otherCar.GetComponent<DrivingManager>();
        projectileUI.AnimateProjectileHitMe(otherDriver.ID);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Retrieve the photon player that controls the given car
    /// </summary>
    /// <param name="car"></param>
    /// <returns></returns>
    public static Player GetPlayer(GameObject car)
    {
        PhotonView view = car.GetComponent<PhotonView>();

        // If the photon view could be retrieved, then return its owner
        if (view) return view.Owner;
        // Throw a missing component exception if the view could not be retrieved
        else throw new MissingComponentException($"{nameof(NetworkPlayer)}: " +
            $"No network player can be controlling this game object " +
            $"because the game object has no photon view");
    }
    /// <summary>
    /// Get the car game object of the local player
    /// </summary>
    /// <returns></returns>
    public static GameObject GetMyCar() => GetCar(PhotonNetwork.LocalPlayer);
    /// <summary>
    /// Get the car game object associated with the given network player
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static GameObject GetCar(Player player)
    {
        if (player.TagObject != null)
        {
            GameObject car = player.TagObject as GameObject;

            // If the cast succeeds then return the car
            if (car) return car;
            // Otherwise throw invalid cast exception
            else throw new System.InvalidCastException($"{nameof(NetworkPlayer)}: " +
                $"Tag object attached to player {player.ActorNumber} " +
                $"is of type '{player.TagObject.GetType().Name}', " +
                $"which is not convertible to type '{nameof(GameObject)}'");
        }
        // Throw null reference exception if tag object is null
        else throw new System.NullReferenceException($"{nameof(NetworkPlayer)}: " +
            $"Tag object of player {player.ActorNumber} is null");
    }
    public static GameObject GetCar(int actor)
    {
        Player player = System.Array.Find(PhotonNetwork.PlayerList, p => p.ActorNumber == actor);

        // If the player is found try to get the game object tag from it
        if (player != null) return GetCar(player);
        // If the player could not be found then throw argument exception
        else throw new System.ArgumentException($"{nameof(NetworkPlayer)}: " +
            $"No network player found with actor number {actor}");
    }
    #endregion
}
