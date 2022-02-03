using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class NetworkLobby : MonoBehaviourPunCallbacks
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Button that forces the game to start")]
    private Button forceStartButton;
    [SerializeField]
    [Tooltip("Reference to the text that displays the number of players who have entered")]
    private TextMeshProUGUI playerText;
    [SerializeField]
    [Tooltip("Parent object for the GUI that displays the countdown before the race loads")]
    private NetworkLobbyCountdown countdown;
    private CarModelSelector carModelSelector;
    #endregion

    #region Monobehaviour Messages
    private void Awake()
    {
        // Open the lobby
        SetLobbyOpen(true);
        
        // Initialize the text that displays players entered
        UpdatePlayerText();

        countdown.Awake();
        CheckLoadRace();

        // When the force button is clicked then call the function that loads the race
        forceStartButton.onClick.AddListener(() => photonView.RPC(nameof(LoadRace), RpcTarget.All));
    }
    #endregion

    #region Photon Callbacks
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("We got here");
        UpdatePlayerText();
        CheckLoadRace();
        UpdatePlayerCarModel();
    }
    // When a player leaves the room, we need to make sure the lobby is open and player text is displayed
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Stop the countdown if it is currently running
        countdown.StopCountdown(this);
        SetLobbyOpen(true);
        UpdatePlayerText();
    }
    #endregion

    #region Remote Procedural Calls
    [PunRPC]
    public void LoadRace()
    {
        // Close the lobby
        SetLobbyOpen(false);

        // Start the countdown routine on the submodule
        countdown.StartCountdown(this);
    }
    #endregion

    #region Private Methods
    // Check if the room is full, and if it is, then load the race
    private void CheckLoadRace()
    {
        // If the room has reached max capacity and this is the master client, prepare to load the race scene
        if (PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            Debug.Log("Maximum players in lobby reached");
            photonView.RPC(nameof(LoadRace), RpcTarget.All);
        }
    }
    private void SetLobbyOpen(bool open)
    {
        forceStartButton.interactable = open;
        playerText.enabled = open;
        PhotonNetwork.CurrentRoom.IsOpen = open;
    }
    private void UpdatePlayerText()
    {
        playerText.enabled = true;
        playerText.text = "Waiting for players to join: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    private void UpdatePlayerCarModel()
    {
        Debug.Log("We got here");
        foreach(Player player in PhotonNetwork.PlayerList)
		{
            Debug.Log("Then we got here");
            MeshFilter carMeshFilter = NetworkPlayer.GetCar(player).GetComponent<MeshFilter>();
            Renderer carMeshRenderer = NetworkPlayer.GetCar(player).GetComponent<Renderer>();
            carMeshFilter.sharedMesh = Resources.Load<Mesh>(carModelSelector.carModels[(int)player.CustomProperties["Car Model"]].name); 
            carMeshRenderer.sharedMaterial = carModelSelector.carMaterials[(int)player.CustomProperties["Car Model"]];
            Debug.Log(player.CustomProperties["Car Model"]);
            Debug.Log(NetworkPlayer.GetCar(player).GetComponent<MeshFilter>());
            Debug.Log(NetworkPlayer.GetCar(player).GetComponent<Renderer>());
        }
            
    }
    #endregion
}
