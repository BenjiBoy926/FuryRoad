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
        List<Player> ranking = NetworkRaceRank.ranking;
        for (int i = 0; i < ranking.Count; i++)
        {
            InstantiateWidget(ranking[i].ActorNumber, i + 1);
        }
    }

    private void InstantiateWidget(int actor, int rank)
    {
        RaceResultWidget clone = Object.Instantiate(widgetPrefab, widgetParent);
        clone.Setup(actor, rank);
    }
}
