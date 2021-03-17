using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceResults : MonoBehaviour
{
    // GUI used to display race results
    private RaceResultsGUI gui;
    // Reference to the finish line checked for the final player ranking
    private FinishLine finishLine;
    // Reference to the script that signals when the race is finished
    private RaceProgress raceProgress;

    private void Start()
    {
        gui = GetComponentInChildren<RaceResultsGUI>();
        gui.gameObject.SetActive(false);

        finishLine = GetComponentInChildren<FinishLine>();

        raceProgress = GetComponent<RaceProgress>();
        raceProgress.onRaceFinished.AddListener(OnRaceFinished);
    }

    public void OnRaceFinished()
    {
        gui.Setup(finishLine);
        // Do a thing when the "next race" button is clicked
    }
}
