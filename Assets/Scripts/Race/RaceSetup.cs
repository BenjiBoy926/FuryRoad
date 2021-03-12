using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class RaceSetup: MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("The base object that holds the information for how many players are in the room")]
    private GameObject waitingParent;
    [SerializeField]
    [Tooltip("Positions where the players spawn when the race begins")]
    private List<Transform> startPositions;
    [SerializeField]
    [Tooltip("This event is invoked if the script determines that the race is ready to begin")]
    private UnityEvent onRaceReady;

    // Text that shows players in room. It should be found somewhere in the waiting parent
    private Text waitingText;

    public static bool raceIsReady
    {
        get
        {
            return PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        SetupLocalPlayer();
        CheckRaceReady();

        // NOTE: it's very important that this gets called AFTER CheckRaceReady
        // because RaceProgress.raceInProgress might get updated afterwards
        SetupWaitingText();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetupLocalPlayer();
        CheckRaceReady();
        SetupWaitingText();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetupWaitingText();
    }

    // Setup the local player by moving them to the spawn position
    private void SetupLocalPlayer()
    {
        int localActor = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        if (localActor < startPositions.Count)
        {
            NetworkHelper.localPlayerManager.transform.forward = startPositions[localActor].forward;
            NetworkHelper.localPlayerManager.transform.position = startPositions[localActor].position;
        }
        else
        {
            Debug.LogError("Actor #" + localActor + " has no spawn position assigned!");
            NetworkHelper.localPlayerManager.transform.position = Vector3.up * 5f;
        }   
    }

    private void CheckRaceReady()
    {
        if (raceIsReady)
        {
            Debug.Log("Race is ready to begin!");
            onRaceReady.Invoke();
        }
    }

    private void SetupWaitingText()
    {
        if(waitingText == null)
        {
            waitingText = waitingParent.GetComponentInChildren<Text>();
        }

        waitingText.text = "Waiting for players to join... (" + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers + ")";
        waitingParent.SetActive(!RaceProgress.raceInProgress);
    }
}
