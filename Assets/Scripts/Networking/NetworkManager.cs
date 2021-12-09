using System.Text;
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
    public static string CurrentRoomString(string tab = "\t", int tabDepth = 0)
    {
        if (PhotonNetwork.IsConnected) return RoomString(PhotonNetwork.CurrentRoom, tab, tabDepth);
        else return "(Not connected)";
    }
    public static string RoomString(Room room, string tab = "\t", int tabDepth = 0)
    {
        if (room != null)
        {
            string str = $"{tab.Repeat(tabDepth)}Room: {room.Name}";
            str += $"\n{tab.Repeat(tabDepth + 1)}Status: ";

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
            str += $"\n{tab.Repeat(tabDepth + 1)}Max Players: {room.MaxPlayers}";
            str += $"\n{tab.Repeat(tabDepth + 1)}Player Ttl: {room.PlayerTtl}";

            // Output all of the players
            str += $"\n{tab.Repeat(tabDepth + 1)}Total Players: {room.Players.Count}\n";
            foreach (Player player in room.Players.Values)
            {
                str += $"{PlayerString(player, tab, tabDepth + 3)}\n";
            }

            return str;
        }
        else return "Room: null";
    }
    public static string PlayerString(Player player, string tab = "\t", int tabDepth = 0)
    {
        string str = $"{tab.Repeat(tabDepth)}Player {player.ActorNumber}";

        // Add a line for stats
        str += $"\n{tab.Repeat(tabDepth + 1)}Status: "; 

        // Add in tags for master client and local player
        if (player.IsMasterClient) str += " <color=blue>(Master)</color>";
        if (player.IsLocal) str += " <color=green>(Myself)</color>";
        if (!player.IsMasterClient && !player.IsLocal) str += "<color=#888888>(Normal Player)</color>";

        // Input the actor number, user id and tag object
        str += $"\n{tab.Repeat(tabDepth + 1)}Tag object: {player.TagObject}";

        // Return the string
        return str;
    }
    #endregion
}
