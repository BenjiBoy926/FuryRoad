using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

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

        // Once the network manager is all set up, load the launcher scene
        settings.launcherScene.LocalLoad();
    }
    // In any case, if the player left the room, they should go back to the launcher screen
    public override void OnLeftRoom()
    {
        settings.launcherScene.LocalLoad();
    }
}
