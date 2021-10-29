using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkCameraSetup : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the camera that this script sets up")]
    private new Camera camera;
    [SerializeField]
    [Tooltip("Reference to the photon view of the camera's root")]
    private PhotonView view;
    [SerializeField]
    [Tooltip("Reference to the component that listens for audio")]
    private AudioListener listener;

    // Start is called before the first frame update
    void Start()
    {
        camera.enabled = view.IsMine;
        listener.enabled = view.IsMine;
    }
}
