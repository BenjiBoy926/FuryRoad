using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class RaceProgress : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("The object that is the parent of the text that displays the player's placement")]
    private GameObject rankParent;
    [SerializeField]
    [Tooltip("Event invoked when the race is finished")]
    private UnityEvent onRaceFinished;

    // Text that displays the user's placement in the race
    private Text rankText;
    // Object that ranks the players who cross the finish line
    private FinishLine finishLine;

    // Determine if the race is in progress
    public static bool raceInProgress
    {
        get; private set;
    }

    private void Start()
    {
        GameObject finishLineObject = GameObject.FindGameObjectWithTag("FinishLine");
        finishLine = finishLineObject.GetComponent<FinishLine>();
        finishLine.onRacerFinished.AddListener(CheckRacerFinished);

        rankText = rankParent.GetComponentInChildren<Text>();
        rankParent.SetActive(false);
    }

    public void OnRaceReady()
    {
        raceInProgress = true;
    }

    public void CheckRacerFinished(int playerFinished)
    {
        if(playerFinished == PhotonNetwork.LocalPlayer.ActorNumber && raceInProgress)
        {
            rankParent.SetActive(true);

            int rank = finishLine.GetLocalPlayerRanking();
            rankText.text = rank.ToString();

            if(rank == 1)
            {
                rankText.text += "st place!";
            }
            else if(rank == 2)
            {
                rankText.text += "nd place!";
            }
            else if(rank == 3)
            {
                rankText.text += "rd place!";
            }
            else
            {
                rankText.text += "th place...";
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            rankParent.SetActive(false);
            raceInProgress = false;
        }
    }
}
