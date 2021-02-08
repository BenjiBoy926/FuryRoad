using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(CameraSetupModule))]
public class CameraSetupDriver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CameraSetupModule setupModule = GetComponent<CameraSetupModule>();
        GameObject localPlayer = (GameObject)PhotonNetwork.LocalPlayer.TagObject;
        setupModule.Setup(localPlayer.transform);
    }
}
