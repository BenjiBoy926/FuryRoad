using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkLobby : MonoBehaviourPunCallbacks
{
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // If the room has reached max capacity and this is the master client, prepare to load the race scene
        if(PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Maximum players in lobby reached");
            Invoke(nameof(LoadRace), 3f);
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Launcher");
    }

    // Load the racing scene
    private void LoadRace()
    {
        Debug.Log("Loading the racing level");
        NetworkManager.settings.raceScene.Load();
    }
}
