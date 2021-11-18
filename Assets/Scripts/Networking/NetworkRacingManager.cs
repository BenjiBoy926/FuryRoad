using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkRacingManager : MonoBehaviourPunCallbacks
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the local racing manager to sync across the network")]
    private RacingManager manager;
    #endregion

    #region Monobehaviour Callbacks
    private void Start()
    {
        
    }
    #endregion

    #region Rpc Targets

    #endregion
}
