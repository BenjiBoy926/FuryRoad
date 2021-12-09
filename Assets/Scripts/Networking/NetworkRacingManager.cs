using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
    private void OnCheckpointPassedRPCSender(DrivingManager driver, RacingCheckpoint checkpoint)
    {
        Player player = NetworkPlayer.GetPlayerControllingCar(driver.gameObject);
        Debug.Log($"Player {PhotonNetwork.LocalPlayer.ActorNumber} detected player {player.ActorNumber} passing checkpoint {checkpoint.Order}, sending rpc");
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
        GameObject player = NetworkPlayer.GetCar(playerActor);
        // Get the checkpoint with the same order
        RacingCheckpoint checkpoint = FindObjectsOfType<RacingCheckpoint>()
            .Where(check => check.Order == checkpointOrder)
            .First();
        // Notify the manager of the checkpoint pass
        manager.OnCheckpointPassed(player.GetComponent<DrivingManager>(), checkpoint);
    }
    [PunRPC]
    public void OnAllRacersFinishedRPCReceiver()
    {
        manager.OnAllRacersFinished();
    }
    #endregion
}
