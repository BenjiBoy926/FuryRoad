using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayerUISetup : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the photon view to check for this player")]
    private PhotonView view;
    [SerializeField]
    [Tooltip("Reference to the root object of the ui")]
    private GameObject uiRoot;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        uiRoot.SetActive(view.IsMine);
    }
    #endregion
}
