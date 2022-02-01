using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using Photon.Pun;
using Photon.Realtime;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    #region Private Properties
    private RoomOptions defaultRoomOptions => new RoomOptions 
    {
        IsOpen = true,
        IsVisible = true,
        MaxPlayers = NetworkManager.settings.maxPlayersPerRace 
    };
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Parent object that contains the controls the player used before starting play")]
    private GameObject playControls;
    [SerializeField]
    [Tooltip("Reference to the button that joins a random room when pressed")]
    private Button quickPlayButton;
    [SerializeField]
    [Tooltip("Reference to the button that joins the test room when pressed")]
    private Button joinTestRoomButton;
    [SerializeField]
    [Tooltip("Parent object for the controls that show the player network connection progress")]
    private GameObject loadingControls;
    #endregion

    #region Private Fields
    public const string gameVersion = "0";
    public const string testRoomName = "Test";
    #endregion

    #region Monobehaviour Messages
    public void Awake()
    {
        // Make the buttons join the correct rooms
        quickPlayButton.onClick.AddListener(JoinRandomRoom);
        joinTestRoomButton.onClick.AddListener(JoinTestRoom);

        // Automatically sync the scene
        PhotonNetwork.AutomaticallySyncScene = true;

        // Disable play controls
        EnablePlayControls(false);

        // Connect to server as soon as we enter the scene
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }
    #endregion

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        EnablePlayControls(true);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Network launcher failed to join a random room, so we will create a random room");
        bool success = PhotonNetwork.CreateRoom(null, defaultRoomOptions);

        // Log result if create room fails
        if (!success)
        {
            Debug.Log("CreateRoom (random) could not be executed on the server");
            EnablePlayControls(true);
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Joining room failed with error: " + message);
        EnablePlayControls(true);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Network launcher successfully loaded a room");
        NetworkManager.settings.lobbyScene.NetworkLoad();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Network launcher disconnected with reason: {0}", cause);
        EnablePlayControls(true);
    }
    #endregion

    #region Public Methods
    public void JoinRandomRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            EnablePlayControls(false);
            bool success = PhotonNetwork.JoinRandomRoom();

            // Debug log if joining the room goes wrong
            if (!success) 
            {
                Debug.Log("JoinRandomRoom could not be called on the server");
                EnablePlayControls(true);
            }
        }
        else
        {
            Debug.Log("Cannot join a random room because we are not connected to a server!");
        }
    }
    public void JoinTestRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            EnablePlayControls(false);
            bool success = PhotonNetwork.JoinOrCreateRoom(testRoomName, defaultRoomOptions, TypedLobby.Default);

            // Debug log if joining fails
            if (!success)
            {
                Debug.Log("JoinOrCreateRoom failed to be called on the server");
                EnablePlayControls(true);
            }
        }
        else
        {
            Debug.Log("Cannot join the test room because we are not connected to a server!");
        }
    }
    #endregion

    #region Private Methods
    private void EnablePlayControls(bool enable)
    {
        playControls.SetActive(enable);
        loadingControls.SetActive(!enable);
    }
    #endregion
}
