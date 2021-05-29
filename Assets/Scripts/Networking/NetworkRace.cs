using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class NetworkRace : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("Information used to perform the countdown before the race begins")]
    private NetworkRaceBegin begin;

    private void Start()
    {
        // Initialize submodules
        begin.Start();

        // As soon as the scene starts, begin the countdown
        // But only for the master client, because it uses RPC to sync across all clients
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(begin.CountdownRoutine(photonView, nameof(StartCountdown), nameof(UpdateCountdown), nameof(FinishCountdown)));
        }
    }

    [PunRPC]
    public void StartCountdown()
    {
        begin.StartCountdown();
    }
    [PunRPC]
    public void UpdateCountdown(int level)
    {
        begin.UpdateCountdown(level);
    }
    [PunRPC]
    public void FinishCountdown()
    {
        begin.FinishCountdown();
    }
}
