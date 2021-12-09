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
        
    }
    #endregion
}
