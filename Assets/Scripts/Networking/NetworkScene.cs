using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

[System.Serializable]
public class NetworkScene
{
    [Tooltip("Name of the scene in the build settings")]
    public string name;
    [Tooltip("True if this scene has a player object and false if not")]
    public bool hasPlayer;
    [TagSelector]
    [Tooltip("Tag of the objects that the players will spawn at")]
    public string spawnPointTag = "Respawn";
    [Tooltip("Extra objects to instantiate on the network when the scene loads")]
    public List<GameObject> additionalObjects;

    // Reference to the player prefab instantiated if the scene has a player
    private GameObject playerPrefab;

    // Get the spawn points
    // Sort the spawn points so that they are in the same order for each client
    public List<GameObject> spawnPoints
    {
        get
        {
            GameObject[] points = GameObject.FindGameObjectsWithTag(spawnPointTag);
            List<GameObject> pointsList = new List<GameObject>(points);
            pointsList.Sort((x, y) => x.GetInstanceID() - y.GetInstanceID());
            return pointsList;
        }
    }

    // Private constructor hides some data from the client for consistency
    private NetworkScene(string name, bool hasPlayer, string spawnPointTag = "Respawn")
    {
        this.name = name;
        this.hasPlayer = hasPlayer;
        this.spawnPointTag = spawnPointTag;
    }
    // Use the two factories below if you want to create a network scene
    public static NetworkScene NonPlayerScene(string name)
    {
        return new NetworkScene(name, false);
    }
    public static NetworkScene PlayerScene(string name, string spawnPointTag = "Respawn")
    {
        return new NetworkScene(name, true, spawnPointTag);
    }
    // On awake, setup the player prefab reference
    // and subscribe to the scene manager loaded event
    public void Awake(GameObject playerPrefab)
    {
        this.playerPrefab = playerPrefab;
        SceneManager.sceneLoaded += CheckInstantiateSceneObjects;
    }
    // Load the scene for all photon clients in the same room
    public void NetworkLoad()
    {
        PhotonNetwork.LoadLevel(name);
    }
    // Load the scene only for the local photon client
    public void LocalLoad()
    {
        SceneManager.LoadScene(name);
    }

    // Check if this scene should have a player.  If it should, instantiate the player
    private void CheckInstantiateSceneObjects(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == name)
        {
            InstantiateAdditionalObjects();
            if (hasPlayer) InstantiatePlayer();
        }
    }

    private void InstantiatePlayer()
    {
        // Get the spawn point at the same index as the local player in the list
        GameObject mySpawn = spawnPoints[NetworkSettings.localPlayerIndex % spawnPoints.Count];        
        // Instantiate the player
        GameObject clone = PhotonNetwork.Instantiate(playerPrefab.name, mySpawn.transform.position, mySpawn.transform.rotation);
        // Assign the player manager to the tag object of the local player
        PlayerManager manager = clone.GetComponent<PlayerManager>();

        // Ensure correct rotation
        manager.movementDriver.movementModule.SetHeading(mySpawn.transform.forward);

        // Set the tag object on the local player
        PhotonNetwork.LocalPlayer.TagObject = manager;
    }

    private void InstantiateAdditionalObjects()
    {
        foreach(GameObject go in additionalObjects)
        {
            PhotonNetwork.Instantiate(go.name, Vector3.zero, Quaternion.identity);
        }
    }
}
