using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class RaceSetup: MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("Positions where the players spawn when the race begins")]
    private List<Transform> startPositions;
    [SerializeField]
    [Tooltip("This event is invoked if the script determines that the race is ready to begin")]
    private UnityEvent raceReady;

    // Start is called before the first frame update
    void Awake()
    {
        SetupLocalPlayer();
        CheckRaceReady();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetupLocalPlayer();
        CheckRaceReady();
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
        if (PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            raceReady.Invoke();
        }
    }
}
