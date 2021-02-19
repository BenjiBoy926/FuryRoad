using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkRace : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject localPlayer = (GameObject)PhotonNetwork.LocalPlayer.TagObject;
        //localPlayer.transform.rotation
        //localPlayer.transform.position = 
    }
}
