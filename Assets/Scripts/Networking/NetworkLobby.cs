﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class NetworkLobby : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("Button used to make the player leave the lobby")]
    private Button leaveButton;
    [SerializeField]
    [Tooltip("Reference to the text that displays the number of players who have entered")]
    private TextMeshProUGUI playerText;
    [SerializeField]
    [Tooltip("Parent object for the GUI that displays the countdown before the race loads")]
    private NetworkLobbyCountdown countdown;

    Coroutine countdownRoutine;

    private void Awake()
    {
        leaveButton.onClick.AddListener(Leave);

        // Open the lobby
        SetLobbyOpen(true);
        
        // Initialize the text that displays players entered
        UpdatePlayerText();

        countdown.Awake();
        CheckLoadRace();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerText();
        CheckLoadRace();
    }
    // When a player leaves the room, we need to make sure the lobby is open and player text is displayed
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Stop the countdown if it is currently running
        countdown.StopCountdown(this);
        SetLobbyOpen(true);
        UpdatePlayerText();
    }
    // Check if the room is full, and if it is, then load the race
    private void CheckLoadRace()
    {
        // If the room has reached max capacity and this is the master client, prepare to load the race scene
        if (PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            Debug.Log("Maximum players in lobby reached");

            // Close the lobby
            SetLobbyOpen(false);

            // Start the countdown routine on the submodule
            countdown.StartCountdown(this);
        }
    }
    private void SetLobbyOpen(bool open)
    {
        leaveButton.interactable = open;
        playerText.enabled = open;
        PhotonNetwork.CurrentRoom.IsOpen = open;
    }
    // Have the player leave the current room
    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void UpdatePlayerText()
    {
        playerText.enabled = true;
        playerText.text = "Waiting for players to join: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
    }
}
