using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class FinishLine : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    public class IntEvent : UnityEvent<int> { }

    [Tooltip("Event called when a racer who has not crossed the finish line before crosses")]
    public IntEvent onRacerFinished;

    // List of racers that have passed the finish line, in the order that they passed
    private List<int> ranking = new List<int>();

    private void OnTriggerEnter(Collider other)
    {
        PlayerManagementModule racer = other.GetComponent<PlayerManagementModule>();
        
        // If this object has a racer on it,
        // and the racer has not already crossed the finish line,
        // add the racer to the ranking and raise the event
        if(racer != null && !ranking.Contains(racer.localActorNumber))
        {
            AddRacer(racer.localActorNumber);
            photonView.RPC("AddRacer", RpcTarget.Others, racer.localActorNumber);
        }
    }

    public int GetLocalPlayerRanking()
    {
        return GetPlayerRanking(NetworkHelper.localPlayerManager);
    }
    public int GetPlayerRanking(PlayerManagementModule player)
    {
        return ranking.IndexOf(player.localActorNumber) + 1;
    }

    [PunRPC]
    public void AddRacer(int racerNumber)
    {
        ranking.Add(racerNumber);
        onRacerFinished.Invoke(racerNumber);

        string message = "Current racer ranking updated! --> ";
        foreach(int rank in ranking)
        {
            message += rank.ToString() + ", ";
        }
        Debug.Log(message);
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if(stream.IsWriting)
    //    {
    //        //string message = "Player " + PhotonNetwork.LocalPlayer.ActorNumber + " sending player ranking: ";
    //        //foreach (int rank in ranking)
    //        //{
    //        //    message += rank.ToString() + ", ";
    //        //}
    //        //Debug.Log(message);

    //        stream.SendNext(ranking.ToArray());
    //    }
    //    else
    //    {
    //        ranking = new List<int>((int[])stream.ReceiveNext());

    //        string message = "Player " + PhotonNetwork.LocalPlayer.ActorNumber + " receiving player ranking from Player " + info.Sender.ActorNumber + ": ";
    //        foreach (int rank in ranking)
    //        {
    //            message += rank.ToString() + ", ";
    //        }
    //        Debug.Log(message);
    //    }
    //}
}
