using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class RacingStart : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the script that manages the race")]
    private RacingManager manager;
    [SerializeField]
    [Tooltip("UI panel that displays information at the start of the race")]
    private GameObject startPanel;
    [SerializeField]
    [Tooltip("Text used to display the start of the race")]
    private TextMeshProUGUI startText;
    [SerializeField]
    [Tooltip("Time delay from the start of the level to the start of the countdown")]
    private float countdownStartDelay = 2f;
    [SerializeField]
    [Tooltip("Number of counts in the starting countdown")]
    private int counts = 3;
    [SerializeField]
    [Tooltip("Time that the start panel lingers after the end of the countdown")]
    private float countdownEndDelay = 2f;
    [SerializeField]
    [Tooltip("Event invoked when all drivers are disabled before the start of the race")]
    private UnityEvent driversDisabledEvent;
    [SerializeField]
    [Tooltip("Event invoked when all drivers are enabled and ready to race")]
    private UnityEvent driversEnabledEvent;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        StartCoroutine(Countdown());
    }
    #endregion

    #region Private Methods
    private IEnumerator Countdown()
    {
        // Wait for one second, cached for slight efficiency
        WaitForSeconds oneSecond = new WaitForSeconds(1f);

        // Enable the starting panel
        startPanel.SetActive(true);
        startText.text = "The race is about to begin!";

        // Wait for end of frame to ensure that all drivers have been properly set up
        yield return new WaitForEndOfFrame();

        // Get all player drivers
        PlayerDriving[] players = FindObjectsOfType<PlayerDriving>();

        // Disable all drivers
        foreach (PlayerDriving player in players)
        {
            player.setControl.Invoke(false);
        }
        driversDisabledEvent.Invoke();

        // Wait for the countdown to start
        yield return new WaitForSeconds(countdownStartDelay);

        // Wait for each count
        for (int i = counts; i > 0; i--)
        {
            startText.text = $"{i}...";
            yield return oneSecond;
        }

        // Set the start text to say "GO"
        startText.text = "GO!!!";

        // Disable all drivers
        foreach (PlayerDriving player in players)
        {
            player.setControl.Invoke(true);
        }
        driversEnabledEvent.Invoke();

        // Wait for the end of the countdown
        yield return new WaitForSeconds(countdownEndDelay);

        // Disable the start panel so it is out of the way
        startPanel.SetActive(false);
    }
    #endregion
}
