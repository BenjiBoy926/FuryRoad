using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RaceResults : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("Reference to the text used to display this player's placement")]
    private Text rankText;

    // Object that ranks the players who cross the finish line
    private FinishLine finishLine;

    private void Start()
    {
        GameObject finishLineObject = GameObject.FindGameObjectWithTag("FinishLine");
        finishLine = finishLineObject.GetComponent<FinishLine>();
        finishLine.onRacerFinished.AddListener(CheckRacerFinished);

        rankText.enabled = false;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        rankText.enabled = false;
    }

    public void CheckRacerFinished(int playerFinished)
    {
        if(playerFinished == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            rankText.enabled = true;

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
}
