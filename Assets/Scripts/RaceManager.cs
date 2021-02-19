using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RaceManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Positions where the players spawn when the race begins")]
    private List<Transform> startPositions;
    private List<RacerTag> ranking = new List<RacerTag>();

    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
