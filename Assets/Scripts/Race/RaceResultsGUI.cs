using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceResultsGUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Prefab of the widget used to display the racer's rank")]
    private RaceResultWidget widgetPrefab;
    [SerializeField]
    [Tooltip("Parent transform for all race results")]
    private Transform widgetParent;

    // List of the widgets that are currently active
    private List<RaceResultWidget> widgets = new List<RaceResultWidget>();

    public void Setup(FinishLine finish)
    {
        gameObject.SetActive(true);

        // Destroy any existing widgets
        foreach(RaceResultWidget widget in widgets)
        {
            Destroy(widget);
        }

        for(int i = finish.ranking.Count - 1; i >= 0; i--)
        {
            int rank = finish.ranking[i];
            InstantiateWidget(finish.GetPlayerRanking(rank), rank);
        }
    }

    private void InstantiateWidget(int actor, int rank)
    {
        RaceResultWidget clone = Instantiate(widgetPrefab, widgetParent);
        clone.Setup(actor, rank);
        widgets.Add(clone);
    }
}
