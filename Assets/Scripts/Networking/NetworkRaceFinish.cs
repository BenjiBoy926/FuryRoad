using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

[System.Serializable]
public class NetworkRaceFinish
{
    [SerializeField]
    [Tooltip("Delay after the last racer finishes before the next scene is loaded")]
    private float loadDelay = 3f;
    [SerializeField]
    [Tooltip("UI that displays when all racers are finished with the race")]
    private NetworkRaceFinishUI ui;

    public void Start()
    {
        ui.Start();
    }
    public IEnumerator RaceFinishRoutine()
    {
        // Update the UI
        ui.SetActive(true);

        yield return new WaitForSeconds(loadDelay);

        // If we are the master client, we will load the results scene for all clients
        if(PhotonNetwork.IsMasterClient)
        {
            NetworkManager.settings.resultsScene.NetworkLoad();
        }
    }
}
