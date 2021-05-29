﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManagementModule : MonoBehaviour, IPunInstantiateMagicCallback
{
    [SerializeField]
    [Tooltip("Reference to the script that drives player movement")]
    private PlayerMovementDriver3D m_MovementDriver;
    [SerializeField]
    [Tooltip("Reference to the rigidbody of the car")]
    private Rigidbody m_Rb;

    public PlayerMovementDriver3D movementDriver => m_MovementDriver;
    public Rigidbody rb => m_Rb;

    // Get the index of this player in the list of network players
    public int playerIndex
    {
        get
        {
            List<Player> players = new List<Player>(PhotonNetwork.PlayerList);
            return players.FindIndex(x => GetModule(x) == this);
        }
    }

    // Get the player management module attached to the local player
    public static PlayerManagementModule local
    {
        get
        {
            return GetModule(PhotonNetwork.LocalPlayer);
        }
    }

    // Get the tag object of the player cast to a player management module
    public static PlayerManagementModule GetModule(Player player)
    {
        return (PlayerManagementModule)player.TagObject;
    }

    public void EnableControl(bool active)
    {
        m_MovementDriver.enabled = active;
    }

    // When the object is instantiated, we need to set the tag object on the player for this client
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this;
    }
}