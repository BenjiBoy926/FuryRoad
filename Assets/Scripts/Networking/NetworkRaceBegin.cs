using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

[System.Serializable]
public class NetworkRaceBegin
{
    [SerializeField]
    [Tooltip("Identify the number of counts in the countdown")]
    private int numCounts = 3;
    [SerializeField]
    [Tooltip("Wait time before the countdown begins")]
    private float startWait = 2f;
    [SerializeField]
    [Tooltip("Time between each count in the countdown")]
    private float countWait = 1f;
    [SerializeField]
    [Tooltip("Time after all counts to wait")]
    private float finishWait = 1f;

    // Countdown update sound
    // Countdown finish sound

    [Header("GUI")]

    [SerializeField]
    [TagSelector]
    [Tooltip("Tag on the object that handles global GUI")]
    private string globalUITag = "GlobalUI";

    private NetworkRaceBeginGUI ui;

    public void Start()
    {
        ui = GameObject.FindGameObjectWithTag(globalUITag).GetComponentInChildren<NetworkRaceBeginGUI>();
    }

    public IEnumerator CountdownRoutine(PhotonView view, string startRPC, string updateRPC, string finishRPC)
    {
        WaitForSeconds wait = new WaitForSeconds(countWait);

        // Remote procedure call to all clients to start the countdown
        view.RPC(startRPC, RpcTarget.All);

        // Wait for the start wait
        yield return new WaitForSeconds(startWait);

        for(int i = 0; i < numCounts; i++)
        {
            // Remote procedural call to all cars to update the countdown
            view.RPC(updateRPC, RpcTarget.All, i);

            // Wait one second
            yield return wait;
        }

        yield return new WaitForSeconds(finishWait);

        // Remote procedural call to all cars to finish the countdown
        view.RPC(finishRPC, RpcTarget.All);
    }

    public void StartCountdown()
    {
        PlayerManagementModule.local.EnableControl(false);
        ui.StartCountdown();
    }

    public void UpdateCountdown(int count)
    {
        Debug.Log(count);

        // If this is the final count, then enable control for the player
        if (count >= (numCounts - 1))
        {
            PlayerManagementModule.local.EnableControl(true);
        }
        ui.UpdateCountdown(count);
    }

    public void FinishCountdown()
    {
        ui.FinishCountdown();
    }
}
