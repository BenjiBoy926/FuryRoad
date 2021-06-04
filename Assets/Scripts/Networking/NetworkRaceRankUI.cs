using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using TMPro;

[System.Serializable]
public class NetworkRaceRankUI
{
    [SerializeField]
    [Tooltip("Root object of the rank UI to enable/disable")]
    private GameObject root;
    [SerializeField]
    [Tooltip("Text that displays the rank when the racer finishes")]
    private TextMeshProUGUI text;

    public void Start()
    {
        root.SetActive(false);
    }

    public void OnRacerFinished(Player racerWhoFinished, int rank)
    {
        // We should check if the local client is the one who finished the race
        if(racerWhoFinished == PhotonNetwork.LocalPlayer)
        {
            root.SetActive(true);
            text.text = RaceHelper.OrdinalString(rank + 1) + " place!";
        }
    }
}
