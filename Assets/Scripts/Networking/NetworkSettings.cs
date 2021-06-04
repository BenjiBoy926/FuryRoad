using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class NetworkSettings
{
    [Tooltip("Game object to instantiate as the player")]
    public GameObject playerPrefab;
    [Tooltip("Maximum number of players allowed in a race at one time")]
    public byte maxPlayersPerRace = 4;

    [Header("Network Scenes")]

    [Tooltip("Name of the scene that launches the network")]
    public NetworkScene launcherScene = NetworkScene.NonPlayerScene("Launcher");
    [Tooltip("Name of the scene loaded for the lobby")]
    public NetworkScene lobbyScene = NetworkScene.PlayerScene("Lobby");
    [Tooltip("Name of the scene loaded for the race")]
    public NetworkScene raceScene = NetworkScene.PlayerScene("LatestTrack");
    [Tooltip("Name of the scene loaded to display the results for a race")]
    public NetworkScene resultsScene = NetworkScene.NonPlayerScene("Results");

    //public static int localPlayerIndex
    // Return a list with the names of all the scenes that have player objects in them
    public List<NetworkScene> scenesWithPlayer
    {
        get
        {
            List<NetworkScene> scenes = new List<NetworkScene>();

            if (launcherScene.hasPlayer) scenes.Add(launcherScene);
            if (lobbyScene.hasPlayer) scenes.Add(lobbyScene);
            if (raceScene.hasPlayer) scenes.Add(raceScene);
            if (resultsScene.hasPlayer) scenes.Add(resultsScene);

            return scenes;
        }
    }
    // Get the index of the local player in the list
    public static int localPlayerIndex
    {
        get
        {
            List<Player> players = new List<Player>(PhotonNetwork.PlayerList);
            return players.IndexOf(PhotonNetwork.LocalPlayer);
        }
    }
    public void Awake()
    {
        launcherScene.Awake(playerPrefab);
        lobbyScene.Awake(playerPrefab);
        raceScene.Awake(playerPrefab);
        resultsScene.Awake(playerPrefab);
    }
}