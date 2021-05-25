using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DebugScript : MonoBehaviour
{
    private void Start()
    {
        ReportPlayerTagObjects("Objects on start:");
        StartCoroutine(DelayLog());
    }

    IEnumerator DelayLog()
    {
        yield return new WaitForSeconds(5f);
        ReportPlayerTagObjects("Objects after a few seconds:");
    }

    private void ReportPlayerTagObjects(string intro)
    {
        Debug.Log(intro);
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log("Player #" + player.ActorNumber + " game object: " + player.TagObject);
        }
    }
}
