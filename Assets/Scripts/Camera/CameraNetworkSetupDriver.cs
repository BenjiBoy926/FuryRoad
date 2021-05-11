using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(CameraSetupModule))]
public class CameraNetworkSetupDriver : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the photon view of the camera's root")]
    private PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().enabled = view.IsMine;
        GetComponent<AudioListener>().enabled = view.IsMine;
    }
}
