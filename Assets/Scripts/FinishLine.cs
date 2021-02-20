using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class FinishLine : MonoBehaviourPunCallbacks, IPunObservable
{
    [System.Serializable]
    public class RacerTagEvent : UnityEvent<RacerTag> { }

    [SerializeField]
    [Tooltip("Event called when a racer who has not crossed the finish line before crosses")]
    private RacerTagEvent onRacerFinished;

    // List of racers that have passed the finish line, in the order that they passed
    private List<RacerTag> ranking;

    private void OnTriggerEnter(Collider other)
    {
        RacerTag racer = other.GetComponent<RacerTag>();
        
        // If this object has a racer on it,
        // and the racer has not already crossed the finish line,
        // add the racer to the ranking and raise the event
        if(racer != null && !ranking.Contains(racer))
        {
            ranking.Add(racer);
            onRacerFinished.Invoke(racer);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(ranking);
        }
        else
        {
            ranking = (List<RacerTag>)stream.ReceiveNext();
        }
    }
}
