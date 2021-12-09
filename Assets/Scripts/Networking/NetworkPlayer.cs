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
    [Tooltip("List of game objects to enable/disable depending on ownership of the photon view provided")]
    private GameObject[] networkSensitiveObjects;
    [SerializeField]
    [Tooltip("List of components to enable/disable depending on ownership of the photon view provided")]
    private MonoBehaviour[] networkSensitiveBehaviours;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        foreach(GameObject obj in networkSensitiveObjects)
        {
            obj.SetActive(photonView.IsMine);
        }
        foreach(MonoBehaviour behaviour in networkSensitiveBehaviours)
        {
            behaviour.enabled = photonView.IsMine;
        }
    }
    #endregion

    #region Interface Implementation
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = gameObject;
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
