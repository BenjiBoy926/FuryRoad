using UnityEngine;
using Photon.Pun;

public class NetworkHelper : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Game object to instantiate as the player")]
    private GameObject playerPrefab;

    private PlayerManagementModule m_LocalPlayerManager;

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
                PhotonNetwork.LocalPlayer.TagObject = playerInstance;
                DontDestroyOnLoad(playerInstance);
                instance.m_LocalPlayerManager = playerInstance.GetComponent<PlayerManagementModule>();

                if(instance.m_LocalPlayerManager == null)
                {
                    throw new MissingComponentException("No player management script found on instance of player prefab!");
                }
            }

            return instance.m_LocalPlayerManager;
        }
    }
}