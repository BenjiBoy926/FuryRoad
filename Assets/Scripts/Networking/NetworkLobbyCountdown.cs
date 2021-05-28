using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

[System.Serializable]
public class NetworkLobbyCountdown
{
    [SerializeField]
    [Tooltip("Object that is the parent of all countdown GUI")]
    private GameObject guiParent;
    [SerializeField]
    [Tooltip("Text that displays the countdown")]
    private TextMeshProUGUI numberText;

    public void Awake()
    {
        guiParent.SetActive(false);
    }

    public IEnumerator CountdownRoutine()
    {
        // Initialize the wait
        WaitForSeconds wait = new WaitForSeconds(1f);

        // Re-enable the gui parent
        guiParent.SetActive(true);

        for(int i = 3; i >= 1; i--)
        {
            numberText.text = i.ToString();
            // Play a sound!
            yield return wait;
        }

        // If this is the master client, then load the race!
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Loading the racing level");
            NetworkManager.settings.raceScene.NetworkLoad();
        }
    }
}
