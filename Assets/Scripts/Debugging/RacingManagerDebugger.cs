using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RacingManagerDebugger : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the racing manager to debug")]
    private RacingManager manager;
    [SerializeField]
    [Tooltip("Reference to the text used to display the manager info")]
    private TextMeshProUGUI display;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        manager.CheckpointPassedEvent.AddListener((player, checkpoint) => UpdateDisplay());
    }
    #endregion

    #region Private Methods
    private void UpdateDisplay()
    {
        string displayText = "";   
        foreach(KeyValuePair<PlayerManager, RacingLapData> playerLapData in manager.PlayerLapData)
        {
            displayText += "Player #" + playerLapData.Key.networkIndex;
            displayText += "\n\tCurrent Checkpoint: ";

            // Display current checkpoint number or "none"
            RacingCheckpoint currentCheckpoint = playerLapData.Value.CurrentCheckpoint;
            if (currentCheckpoint) displayText += "#" + currentCheckpoint.Order;
            else displayText += "(none)";

            // Display current lap
            displayText += "\n\tCurrent Lap: " + playerLapData.Value.CurrentLap;
            displayText += "\n";
        }

        // Set the text on the display
        display.text = displayText;
    }
    #endregion
}
