using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("Parent object that contains the controls the player used before starting play")]
    private GameObject playControls;
    [SerializeField]
    [Tooltip("Parent object for the controls that show the player network connection progress")]
    private GameObject loadingControls;
    [SerializeField]
    [Tooltip("If true, connect as soon as the scene loads")]
    private bool connectOnAwake;

    private bool isConnecting;

    public const string gameVersion = "0";

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        EnablePlayControls(true);

        if (connectOnAwake) Connect();
    }

    public void Connect()
    {
        EnablePlayControls(false);

        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    private void EnablePlayControls(bool enable)
    {
        playControls.SetActive(enable);
        loadingControls.SetActive(!enable);
    }

    // PUN callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("Network launcher connected to master");

        // Check to make sure we are trying to connect to a room
        if(isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Network launcher disconnected with reason: {0}", cause);
        EnablePlayControls(true);
        isConnecting = false;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Network launcher failed to join a random room, so we will create one");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = NetworkManager.settings.maxPlayersPerRace, PlayerTtl = 0 });
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Network launcher successfully loaded a room");
        NetworkManager.settings.lobbyScene.NetworkLoad();
    }
}
