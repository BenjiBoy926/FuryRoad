using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

[System.Serializable]
public class NetworkRaceRanker
{
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag on the object that has a finish line script attached")]
    private string finishLineTag = "FinishLine";

    // Reference to the script that raises an event anytime a player passes the finish line
    private FinishLine finishLine;
    // Photon view to target with the rpc
    private PhotonView targetView;
    // Name of the RPC to callback on the photon view
    private string rpcCallback;

    public void Start(PhotonView targetView, string rpcCallback)
    {
        // Assign local variables
        this.targetView = targetView;
        this.rpcCallback = rpcCallback;

        // Get the finish line and subscribe to the event raised when a racer finished
        finishLine = GameObject.FindGameObjectWithTag(finishLineTag).GetComponentInChildren<FinishLine>();
        finishLine.onRacerFinished.AddListener(BroadcastRacerFinished);
    }

    private void BroadcastRacerFinished(PlayerManager player)
    {
        targetView.RPC(rpcCallback, RpcTarget.All, player.index);
    }
}
