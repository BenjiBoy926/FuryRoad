using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkLobby : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("Prefab of the player to instantiate")]
    private GameObject playerPrefab;

    private void Awake()
    {
        NetworkHelper.localObject.transform.position = Vector3.up * 5f;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Maximum number of players in room has been reached.  Loading the racing level");
            PhotonNetwork.LoadLevel("Race");
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Launcher");
    }
}
