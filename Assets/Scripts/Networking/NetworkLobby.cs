using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkLobby : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("Button used to make the player leave the lobby")]
    private Button leaveButton;

    private void Awake()
    {
        leaveButton.onClick.AddListener(Leave);
        CheckLoadRace();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CheckLoadRace();
    }
    // Check if the room is full, and if it is, then load the race
    private void CheckLoadRace()
    {
        // If the room has reached max capacity and this is the master client, prepare to load the race scene
        if (PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            Debug.Log("Maximum players in lobby reached");
            leaveButton.interactable = false;

            // If this is the master client, then load the race after a few seconds
            if (PhotonNetwork.IsMasterClient)
            {
                Invoke(nameof(LoadRace), 3f);
            }
        }
    }
    // Load the racing scene
    private void LoadRace()
    {
        Debug.Log("Loading the racing level");
        NetworkManager.settings.raceScene.NetworkLoad();
    }
    // Have the player leave the current room
    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }
}
