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
    [Tooltip("Event invoked when the race is finished")]
    public UnityEvent onRaceFinished;

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
        finishLine = GetComponentInChildren<FinishLine>();
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
            rankText.text = RaceHelper.OrdinalString(rank);

            if(rank <= 3)
            {
                rankText.text += " place!";
            }
            else
            {
                rankText.text += " place...";
            }
        }

        CheckAllRacersFinished();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            rankParent.SetActive(false);
            raceInProgress = false;
        }
    }

    //[PunRPC]
    public void CheckAllRacersFinished()
    {
        if(finishLine.allRacersFinished)
        {
            AllRacersFinished();
        }
    }

    private void AllRacersFinished()
    {
        rankParent.SetActive(false);
        onRaceFinished.Invoke();
    }


}
