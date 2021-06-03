using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class NetworkRace : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("Information used to perform the countdown before the race begins")]
    private NetworkRaceBegin begin;
    [SerializeField]
    [Tooltip("Information used to setup the way that the racer ranking updates")]
    private NetworkRaceRanker ranker;

    // List of the ranking for the most recent race
    // The list contains the indices of the player in the list of Photon players
    public static List<int> ranking;

    private void Start()
    {
        // When a new race begins, you must clear the ranking for this client
        ranking = new List<int>();

        // Initialize submodules
        ranker.Start(photonView, nameof(OnRacerFinished));

        // As soon as the scene starts, begin the countdown
        // But only for the master client, because it uses RPC to sync across all clients
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(begin.CountdownRoutine(photonView, nameof(StartCountdown), nameof(UpdateCountdown), nameof(FinishCountdown)));
        }
    }

    // RACE BEGIN RPC
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

    // RACE END RPC
    [PunRPC]
    public void OnRacerFinished(int playerIndex)
    {
        if (!ranking.Contains(playerIndex))
        {
            ranking.Add(playerIndex);

            // Check if all racers have finished
            if(ranking.Count >= PhotonNetwork.CurrentRoom.PlayerCount)
            {
                // End the race!
            }
        }
    }
}
