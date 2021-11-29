using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RacingResultsLoader : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the racing manager that determines when the race is over")]
    private RacingManager manager;
    [SerializeField]
    [Tooltip("Time it takes to countdown down to the race results")]
    private int countdown = 5;
    [SerializeField]
    [Tooltip("Text to update on each count of the coundown. Use '{0}' in the position of the string " +
        "where you want to display the count in the countdown")]
    private string displayText;
    [SerializeField]
    [Tooltip("Object used to display the text")]
    private TextMeshProUGUI textComponent;
    [SerializeField]
    [Tooltip("Root object of the text component display")]
    private GameObject textObject;
    #endregion

    #region Private Fields
    private Coroutine countdownRoutine;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        textObject.SetActive(false);
        manager.AllRacersFinishedEvent.AddListener(StartCountdown);
    }
    #endregion

    #region Private Methods
    private void StartCountdown()
    {
        textObject.SetActive(true);
        
        // Stop the coroutine and start it again
        if (countdownRoutine != null) StopCoroutine(countdownRoutine);
        countdownRoutine = StartCoroutine(CountdownRoutine());
    }
    private IEnumerator CountdownRoutine()
    {
        // Cache the wait for slight efficiency
        WaitForSeconds wait = new WaitForSeconds(1f);

        // Perform the countdown and update the text every second
        for(int i = 0; i < countdown; i++)
        {
            textComponent.text = string.Format(displayText, countdown - i);
            yield return wait;
        }

        // Load the results
        NetworkRaceResults.LoadResults(manager.Ranking);
    }
    #endregion
}
