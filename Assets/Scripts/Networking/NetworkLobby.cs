using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkLobby : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Prefab of the player to instantiate")]
    private GameObject playerPrefab;


    private void Awake()
    {
        if(PhotonNetwork.LocalPlayer.TagObject == null)
        {
            PhotonNetwork.LocalPlayer.TagObject = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.up * 5f, Quaternion.identity);
            DontDestroyOnLoad((GameObject)PhotonNetwork.LocalPlayer.TagObject);
        }
    }
}
