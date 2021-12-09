using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class NetworkRaceRank
{
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag on the object that has a finish line script attached")]
    private string finishLineTag = "FinishLine";
    [SerializeField]
    [Tooltip("Data used to manage the ui")]
    private NetworkRaceRankUI ui;
    [Tooltip("Event invoked when all racers have finished the race")]
    public UnityEvent allRacersFinished;

    // Reference to the script that raises an event anytime a player passes the finish line
    private FinishLine finishLine;
    // Photon view to target with the rpc
    private PhotonView targetView;
    // Name of the RPC to callback on the photon view
    private string rpcCallback;

    // Ranking of the players in the previous race
    // Holds the index of the player in PhotonNetwork.PlayerList
    public static List<Player> ranking;

    public void Start(PhotonView targetView, string rpcCallback)
    {
        // Initialize ui
        ui.Start();

        // Create a new list
        ranking = new List<Player>();

        // Assign local variables
        this.targetView = targetView;
        this.rpcCallback = rpcCallback;

        // Get the finish line object
        GameObject finishLineObject = GameObject.FindGameObjectWithTag(finishLineTag);

        // If finish line is found, then try to get the component
        if(finishLineObject)
        {
            finishLine = finishLineObject.GetComponentInChildren<FinishLine>(true);

            // If the finish line could be got, then add a listener to the racer finished event
            if(finishLine)
            {
                finishLine.onRacerFinished.AddListener(BroadcastRacerFinished);
            }
            else Debug.Log($"{nameof(NetworkRaceRank)}: finish line object '{finishLineObject}' " +
                $"has no component of type '{nameof(FinishLine)}' attached to it or any of it's children");
        }
        // No finish line was found, so do a debug log
        else Debug.Log($"{nameof(NetworkRaceRank)}: no object with the tag '{finishLineTag}' could be found in the scene, " +
            $"so the racers will not be able to finish the race");
    }

    // When a racer crosses the finish line, broadcast to all clients that a racer has finished
    private void BroadcastRacerFinished(PlayerManager player)
    {
        // This is deprecated now, sorry!
        // targetView.RPC(rpcCallback, RpcTarget.All, player.networkIndex);
    }

    // Invoked by the RPC of the parent
    // We can't pass the PlayerManager directly because Photon cannot properly serialize it for the RPC
    public void OnRacerFinished(int playerIndex)
    {
        Player player = PhotonNetwork.PlayerList[playerIndex];

        if (!ranking.Contains(player))
        {
            ranking.Add(player);

            // Callback on the ui
            ui.OnRacerFinished(player, ranking.Count - 1);
            
            // Check if all of the racers have finished
            CheckAllRacersFinished();
        }
    }
    // When a player leaves, remove it from the ranking
    public void OnPlayerLeftRoom(Player player)
    {
        ranking.Remove(player);
        CheckAllRacersFinished();
    }
    // Check if all the racers have finished
    private void CheckAllRacersFinished()
    {
        // Check if all racers have finished
        if (ranking.Count >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            allRacersFinished.Invoke();
        }
    }
}
