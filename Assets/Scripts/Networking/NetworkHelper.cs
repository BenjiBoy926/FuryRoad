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

    public static GameObject localObject
    {
        get
        {
            if(PhotonNetwork.LocalPlayer.TagObject == null)
            {
                GameObject playerInstance = PhotonNetwork.Instantiate(instance.playerPrefab.name, Vector3.zero, instance.playerPrefab.transform.rotation);
                PhotonNetwork.LocalPlayer.TagObject = playerInstance;
                DontDestroyOnLoad(playerInstance);
            }
            return (GameObject)PhotonNetwork.LocalPlayer.TagObject;
        }
    }
}