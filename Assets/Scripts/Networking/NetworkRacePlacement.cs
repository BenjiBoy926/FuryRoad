using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using Photon.Pun;
using Photon.Realtime;

public class NetworkRacePlacement
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

    // Ranking of the players in the previous race
    // Holds the index of the player in PhotonNetwork.PlayerList
    private List<Player> ranking;

    //Holds position of FinishLine
    private Vector3 finishLinePosition;

    //Private call to the PlayerUIManager to update UI with correct placement

    // Start is called before the first frame update
    public void Start(PhotonView targetView, string rpcCallback)
    {
        // Create a new list and add players to that list
        ranking = new List<Player>();
        foreach (Player p in PhotonNetwork.PlayerList){
            ranking.Add(p);
        }

        // Assign local variables
        this.targetView = targetView;
        this.rpcCallback = rpcCallback;

        // Get the finish line and position of the finish line
        finishLine = GameObject.FindGameObjectWithTag(finishLineTag).GetComponentInChildren<FinishLine>();
        finishLinePosition = finishLine.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        calculateDistanceFromFinishLine(ranking, finishLinePosition);

    }

    private void calculateDistanceFromFinishLine(List<Player> playerList, Vector3 finishLine){
        // Calculate the distance from player to finish line 
        foreach (Player p in playerList){
            
        }
    }
}
