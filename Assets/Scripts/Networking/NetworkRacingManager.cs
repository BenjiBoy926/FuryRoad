﻿using System.Linq;
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
        manager.CheckpointPassedEvent.AddListener(OnCheckpointPassedRPCSender);
        manager.AllRacersFinishedEvent.AddListener(OnAllRacersFinishedRPCSender);
    }
    #endregion

    #region Private Methods
    private void OnCheckpointPassedRPCSender(PlayerManager player, RacingCheckpoint checkpoint)
    {
        // This appears to result in a double count of the player passing the second to last checkpoint, 
        // resulting in an early win
        // photonView.RPC(nameof(OnCheckpointPassedRPCReceiver), RpcTarget.Others, player.networkActor, checkpoint.Order);
    }
    private void OnAllRacersFinishedRPCSender()
    {
        photonView.RPC(nameof(OnAllRacersFinishedRPCReceiver), RpcTarget.Others);
    }
    #endregion

    #region Rpc Targets
    [PunRPC]
    public void OnCheckpointPassedRPCReceiver(int playerActor, int checkpointOrder)
    {
        // Get the player manager with the same actor number
        PlayerManager player = PlayerManager.GetByActor(playerActor);
        // Get the checkpoint with the same order
        RacingCheckpoint checkpoint = FindObjectsOfType<RacingCheckpoint>()
            .Where(check => check.Order == checkpointOrder)
            .First();
        // Notify the manager of the checkpoint pass
        manager.OnCheckpointPassed(player, checkpoint);
    }
    [PunRPC]
    public void OnAllRacersFinishedRPCReceiver()
    {
        manager.OnAllRacersFinished();
    }
    #endregion
}
