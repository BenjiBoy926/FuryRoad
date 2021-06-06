using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkRaceCreator : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the prefab to instantiate that manages the race")]
    private GameObject raceManagerPrefab;

    private void Awake()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(raceManagerPrefab.name, Vector3.zero, Quaternion.identity);
        }
    }
}
