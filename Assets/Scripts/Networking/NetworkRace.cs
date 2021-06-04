using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class NetworkRace : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("Information used to perform the countdown before the race begins")]
    private NetworkRaceStart start;
    [SerializeField]
    [Tooltip("Information used to setup the way that the racer ranking updates")]
    private NetworkRaceRank ranker;
    [SerializeField]
    [Tooltip("Information used to end the race once all racers are finished")]
    private NetworkRaceFinish finish;

    private void Start()
    {
        // Initialize submodules
        ranker.Start(photonView, nameof(OnRacerFinished));
        finish.Start();

        // Add a callback function when all players have finished and are ranked
        ranker.allRacersFinished.AddListener(FinishRace);

        // As soon as the scene starts, begin the countdown
        // But only for the master client, because it uses RPC to sync across all clients
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(start.CountdownRoutine(photonView, nameof(StartCountdown), nameof(UpdateCountdown), nameof(FinishCountdown)));
        }
    }

    // RACE BEGIN RPC
    [PunRPC]
    public void StartCountdown()
    {
        start.StartCountdown();
    }
    [PunRPC]
    public void UpdateCountdown(int level)
    {
        start.UpdateCountdown(level);
    }
    [PunRPC]
    public void FinishCountdown()
    {
        start.FinishCountdown();
    }

    // RACE RANK RPC
    [PunRPC]
    public void OnRacerFinished(int playerIndex)
    {
        ranker.OnRacerFinished(playerIndex);   
    }
    private void FinishRace()
    {
        StartCoroutine(finish.RaceFinishRoutine());
    }
}
