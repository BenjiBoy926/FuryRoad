using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacksSingleton<NetworkManager>
{
    #region Public Properties
    public static NetworkSettings settings => Instance.m_Settings;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Global settings for network operations")]
    private NetworkSettings m_Settings;
    #endregion

    #region Monobehaviour Messages
    private void Awake()
    {
        // Initiaize all submodules
        m_Settings.Awake();
    }
    #endregion

    #region Photon Networking Messages
    // In any case, if the player left the room, they should go back to the launcher screen
    public override void OnLeftRoom()
    {
        settings.launcherScene.LocalLoad();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected with cause: {cause}");
        settings.launcherScene.LocalLoad();
    }
    #endregion
}
