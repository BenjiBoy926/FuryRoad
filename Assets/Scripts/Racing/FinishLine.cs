using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

// Simple class that detects when a player crosses and invokes a public event
public class FinishLine : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    public class PlayerManagerEvent : UnityEvent<PlayerManager> { }

    [Tooltip("Event called when a racer who has not crossed the finish line before crosses")]
    public PlayerManagerEvent onRacerFinished;

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager racer = other.GetComponentInParent<PlayerManager>();

        // If this object has a racer on it, invoke the event
        if (racer != null) onRacerFinished.Invoke(racer);
    }
}
