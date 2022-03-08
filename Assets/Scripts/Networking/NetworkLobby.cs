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
        UpdatePlayerText();
        CheckLoadRace();
        //UpdatePlayerCarModel();
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

    /*private void UpdatePlayerCarModel()
    {
        Debug.Log("We got here");
        foreach(Player player in PhotonNetwork.PlayerList)
		{
            if(player.CustomProperties.ContainsKey("Car Model"))
            {
                int carModel = (int)player.CustomProperties["Car Model"];
                Debug.Log("YAY! Then we got here");
                Debug.Log(carModel + "This from here");
                Debug.Log(carModelsList[carModel].name);
            }
            Debug.Log("Then we got here");
            MeshFilter carMeshFilter = NetworkPlayer.GetCar(player).GetComponentInChildren<MeshFilter>();
            Renderer carMeshRenderer = NetworkPlayer.GetCar(player).GetComponentInChildren<Renderer>();
            Debug.Log(carMeshFilter);
            Debug.Log(carMeshRenderer);

            carMeshFilter.sharedMesh = Resources.Load<Mesh>(carModelsList[(int)player.CustomProperties["Car Model"]].name); 
            carMeshRenderer.sharedMaterial = carMaterialsList[(int)player.CustomProperties["Car Model"]];
        }
            
    }*/
    #endregion
}
