using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkLobby : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("Prefab of the player to instantiate")]
    private GameObject playerPrefab;
    [SerializeField]
    [Tooltip("Name of the scene where the racers race")]
    private string raceSceneName = "Race";

    private void Start()
    {
        NetworkHelper.localObject.transform.position = Vector3.up * 5f;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Maximum players in lobby reached");
            LoadRace();
            //Invoke("LoadRace", 0.5f);
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Launcher");
    }

    private void LoadRace()
    {
        Debug.Log("Loading the racing level");
        PhotonNetwork.LoadLevel(raceSceneName);
    }
}
