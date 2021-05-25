using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("Global settings for network operations")]
    private NetworkSettings m_Settings;

    public static NetworkManager instance;
    public static NetworkSettings settings => instance.m_Settings;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        // Initiaize all submodules
        m_Settings.Awake();
    }
}
