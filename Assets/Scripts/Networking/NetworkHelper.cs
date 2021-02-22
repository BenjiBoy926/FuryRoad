using UnityEngine;
using Photon.Pun;

public class NetworkHelper : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Game object to instantiate as the player")]
    private GameObject playerPrefab;

    public static NetworkHelper instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public static PlayerManagementModule localPlayerManager
    {
        get
        {
            // If there is no tag object on the local network player, instantiate it
            if (PhotonNetwork.LocalPlayer.TagObject == null)
            {
                GameObject playerInstance = PhotonNetwork.Instantiate(instance.playerPrefab.name, Vector3.zero, instance.playerPrefab.transform.rotation);
                DontDestroyOnLoad(playerInstance);
                PhotonNetwork.LocalPlayer.TagObject = GetPlayerManager(playerInstance);
            }

            return (PlayerManagementModule)PhotonNetwork.LocalPlayer.TagObject;
        }
    }

    public static PlayerManagementModule GetPlayerManager(GameObject instance)
    {
        PlayerManagementModule player = instance.GetComponent<PlayerManagementModule>();

        if (player != null)
        {
            return player;
        }
        else
        {
            throw new MissingComponentException("No player management script found on instance of player prefab!");
        }
    }
}