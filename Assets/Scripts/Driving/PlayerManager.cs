using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour, IPunInstantiateMagicCallback
{
    #region Public Typedefs
    [System.Serializable]
    public class IntEvent : UnityEvent<int> { }
    #endregion

    #region Public Properties
    public PlayerDriving movementDriver => m_MovementDriver;
    public Rigidbody rb => m_Rb;
    public UnityEvent<int> PlayerFinishedEvent => playerFinishedEvent;

    // Get the index of this player in the list of network players
    public int networkIndex
    {
        get
        {
            List<Player> players = new List<Player>(PhotonNetwork.PlayerList);
            return players.FindIndex(x => Get(x) == this);
        }
    }
    // Get the player management module attached to the local player
    public static PlayerManager networkLocal
    {
        get
        {
            return Get(PhotonNetwork.LocalPlayer);
        }
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the script that drives player movement")]
    private PlayerDriving m_MovementDriver;
    [SerializeField]
    [Tooltip("Reference to the rigidbody of the car")]
    private Rigidbody m_Rb;
    [SerializeField]
    [Tooltip("Event invoked when the player finishes the race")]
    private IntEvent playerFinishedEvent;
    #endregion

    #region Public Methods
    // Get the tag object of the player cast to a player manager
    public static PlayerManager Get(int index)
    {
        return Get(PhotonNetwork.PlayerList[index]);
    }
    public static PlayerManager Get(Player player)
    {
        return (PlayerManager)player.TagObject;
    }
    // Enable/Disable control of the car
    public void EnableControl(bool active)
    {
        m_MovementDriver.enabled = active;
    }
    public void OnRaceFinished(int rank)
    {
        playerFinishedEvent.Invoke(rank);
    }
    #endregion

    #region Photon Callbacks
    // When the object is instantiated, we need to set the tag object on the player for this client
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this;
    }
    #endregion
}