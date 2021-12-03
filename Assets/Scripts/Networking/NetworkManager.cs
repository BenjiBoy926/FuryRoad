using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacksSingleton<NetworkManager>
{
    #region Public Properties
    public static NetworkSettings settings => Instance.m_Settings;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Toggle used to display the room statistics")]
    private Toggle roomStatisticToggle;
    [SerializeField]
    [Tooltip("Game object used as the panel with all room info")]
    private GameObject roomStatisticPanel;
    [SerializeField]
    [Tooltip("Text used to display the room info")]
    private TextMeshProUGUI roomStatisticText;
    [SerializeField]
    [Tooltip("Global settings for network operations")]
    private NetworkSettings m_Settings;
    #endregion

    #region Monobehaviour Messages
    private void Awake()
    {
        // Enable panel when toggle is enabled
        roomStatisticPanel.SetActive(roomStatisticToggle.isOn);
        roomStatisticToggle.onValueChanged.AddListener(value => roomStatisticPanel.SetActive(value));

        // Initiaize all submodules
        m_Settings.Awake();
    }
    private void Update()
    {
        roomStatisticText.text = CurrentRoomString();
    }
    #endregion

    #region Photon Networking Messages
    // In any case, if the player left the room, they should go back to the launcher screen
    public override void OnLeftRoom()
    {
        settings.launcherScene.LocalLoad();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected with cause: {cause}");
        settings.launcherScene.LocalLoad();
    }
    #endregion

    #region Public Methods
    public static string CurrentRoomString(string newLinePrefix = "")
    {
        if (PhotonNetwork.IsConnected) return RoomString(PhotonNetwork.CurrentRoom, newLinePrefix);
        else return "(Not connected)";
    }
    public static string RoomString(Room room, string newLinePrefix = "")
    {
        if (room != null)
        {
            string str = $"Room: {room.Name}";
            str += $"\n\t{newLinePrefix}Status:      ";

            // Output open or closed for the room
            if (room.IsOpen) str += "<color=green>Open</color>, ";
            else str += "<color=red>Closed</color>, ";
            // Output if room is visible or not
            if (room.IsVisible) str += "<b>Visible</b>, ";
            else str += "<color=#888888>Invisible</color>, ";
            // Output if the room is online or not
            if (room.IsOffline) str += "<color=yellow>Offline</color>";
            else str += "<color=blue>Online</color>";

            // Output some more stats
            str += $"\n\t{newLinePrefix}Max Players: {room.MaxPlayers}";
            str += $"\n\t{newLinePrefix}Player Ttl:  {room.PlayerTtl}";

            // Output all of the players
            str += $"\n\t{newLinePrefix}Total Players: {room.Players.Count}";
            foreach (Player player in room.Players.Values)
            {
                str += $"\n\t\t{newLinePrefix}" + PlayerString(player, $"\t\t{newLinePrefix}");
            }

            return str;
        }
        else return "Room: null";
    }
    public static string PlayerString(Player player, string newLinePrefix)
    {
        string str = $"Player {player.ActorNumber}";

        // Add a line for stats
        str += $"\n\t{newLinePrefix}Status:    "; 

        // Add in tags for master client and local player
        if (player.IsMasterClient) str += " <color=blue>(Master)</color>";
        if (player.IsLocal) str += " <color=green>(Myself)</color>";
        if (!player.IsMasterClient && !player.IsLocal) str += "<color=#888888>(Normal Player)</color>";

        // Input the actor number, user id and tag object
        str += $"\n\t{newLinePrefix}Tag object: {player.TagObject}";

        // Return the string
        return str;
    }
    #endregion
}
