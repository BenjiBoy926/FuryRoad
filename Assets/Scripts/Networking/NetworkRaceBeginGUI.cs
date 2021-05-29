using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class NetworkRaceBeginGUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Root object for all countdown gui")]
    private GameObject root;
    [SerializeField]
    [Tooltip("Text that displays during the countdown")]
    private TextMeshProUGUI text;
    [SerializeField]
    [Tooltip("Text displayed while the race is about to begin")]
    private string beginningText = "The race is about to begin!";
    [SerializeField]
    [Tooltip("List of objects to reveal as we countdown")]
    private List<NetworkRaceCount> counts = new List<NetworkRaceCount>()
    {
        new NetworkRaceCount(null, "Ready..."),
        new NetworkRaceCount(null, "Set..."),
        new NetworkRaceCount(null, "GO!")
    };

    public void StartCountdown()
    {
        root.SetActive(true);

        // Start all counts as inactive
        foreach(NetworkRaceCount count in counts)
        {
            count.SetActive(false, text);
        }
        // Set the beginning text
        text.text = beginningText;
    }

    public void UpdateCountdown(int level)
    {
        // Loop up to either the current level or the number of gui counts,
        // whichever is smaller
        int min = Mathf.Min(level + 1, counts.Count);
        for(int i = 0; i < min; i++)
        {
            counts[i].SetActive(level >= i, text);
        }
    }

    public void FinishCountdown()
    {
        root.SetActive(false);
    }
}
