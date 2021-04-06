using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceResults : MonoBehaviour
{
    [SerializeField]
    // GUI used to display race results
    private RaceResultsGUI gui;

    [SerializeField]
    // Reference to the finish line checked for the final player ranking
    private FinishLine finishLine;

    // Reference to the script that signals when the race is finished
    private RaceProgress raceProgress;

    private void Start()
    {
        gui.gameObject.SetActive(false);

        raceProgress = GetComponent<RaceProgress>();
        raceProgress.onRaceFinished.AddListener(OnRaceFinished);
    }

    public void OnRaceReady()
    {
        gui.gameObject.SetActive(false);
    }

    public void OnRaceFinished()
    {
        gui.Setup(finishLine);
        // Do a thing when the "next race" button is clicked
    }
}
