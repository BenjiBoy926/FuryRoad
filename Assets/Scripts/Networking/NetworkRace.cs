using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkRace : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Tooltip("Positions where the players spawn when the race begins")]
    private List<Transform> startPositions;

    // Start is called before the first frame update
    void Awake()
    {
        SetupLocalPlayer();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetupLocalPlayer();
    }

    // Setup the local player by moving them to the spawn position
    private void SetupLocalPlayer()
    {
        int localActor = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        if (localActor < startPositions.Count)
        {
            NetworkHelper.localObject.transform.forward = startPositions[localActor].forward;
            NetworkHelper.localObject.transform.position = startPositions[localActor].position;
        }
        else
        {
            Debug.LogError("Actor #" + localActor + " has no spawn position assigned!");
            NetworkHelper.localObject.transform.position = Vector3.up * 5f;
        }
    }
}
