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

    // Count the number of players that have finished
    // We only count non-null because players who leave after passing the finish line have their references invalidated
    public static int playersFinished
    {
        get
        {
            return ranking.FindAll(x => x != null).Count;
        }
    }

    public void Start(PhotonView targetView, string rpcCallback)
    {
        // Initialize ui
        ui.Start();

        // Create a new list
        ranking = new List<Player>();

        // Assign local variables
        this.targetView = targetView;
        this.rpcCallback = rpcCallback;

        // Get the finish line and subscribe to the event raised when a racer finished
        finishLine = GameObject.FindGameObjectWithTag(finishLineTag).GetComponentInChildren<FinishLine>();
        finishLine.onRacerFinished.AddListener(BroadcastRacerFinished);
    }

    // When a racer crosses the finish line, broadcast to all clients that a racer has finished
    private void BroadcastRacerFinished(PlayerManager player)
    {
        targetView.RPC(rpcCallback, RpcTarget.All, player.index);
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

            // Check if all racers have finished
            if (playersFinished >= PhotonNetwork.CurrentRoom.PlayerCount)
            {
                allRacersFinished.Invoke();
            }
        }
    }
}
