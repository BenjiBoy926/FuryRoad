using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



using Photon.Pun;
using Photon.Realtime;

using TMPro;

public class PlacementUIManager : MonoBehaviour
{
    public FinishLine Raceranking;
    [SerializeField]
    [Tooltip("Text that displays the placement of racer")]
    private Text text;

    public void UpdateUI(int rank, Player racer){
        if(racer == PhotonNetwork.LocalPlayer)
        {
            text.text = RaceHelper.OrdinalString(rank + 1);
        }
    }
}
