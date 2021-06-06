using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

[System.Serializable]
public class NetworkRaceStart
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
    [Tooltip("Reference to the button that lets the player leave the race")]
    private NetworkLeaveRoomButton leaveButton;
    [SerializeField]
    [Tooltip("Used to manage the begin GUI")]
    private NetworkRaceStartGUI ui;

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
        PlayerManager.local.EnableControl(false);
        ui.StartCountdown();
        leaveButton.interactable = false;
    }

    public void UpdateCountdown(int count)
    {
        // If this is the final count, then enable control for the player
        if (count >= (numCounts - 1))
        {
            PlayerManager.local.EnableControl(true);
        }
        ui.UpdateCountdown(count);
    }

    public void FinishCountdown()
    {
        ui.FinishCountdown();
        leaveButton.interactable = true;
    }
}
