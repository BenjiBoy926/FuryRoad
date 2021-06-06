using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;

[System.Serializable]
public class NetworkRaceResultsGUI
{
    [SerializeField]
    [Tooltip("Prefab of the widget used to display the racer's rank")]
    private RaceResultWidget widgetPrefab;
    [SerializeField]
    [Tooltip("Parent transform for all race results")]
    private Transform widgetParent;

    public void Start()
    {
        // Cache ranking list for efficient use
        List<Player> ranking = NetworkRaceRank.ranking;

        for (int i = 0; i < ranking.Count; i++)
        {
            int number;

            // Check to make sure that the player exists.
            // They might not exist if they left before the results screen loaded
            if (ranking[i] != null) number = ranking[i].ActorNumber;
            else number = -1;

            InstantiateWidget(number, i + 1);
        }
    }

    private void InstantiateWidget(int actor, int rank)
    {
        RaceResultWidget clone = Object.Instantiate(widgetPrefab, widgetParent);
        clone.Setup(actor, rank);
    }
}
