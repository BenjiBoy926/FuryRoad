using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRaceResults : MonoBehaviour
{
    [SerializeField]
    [Tooltip("GUI used to display race results")]
    private NetworkRaceResultsGUI gui;

    private void Start()
    {
        gui.Start();
    }
}
