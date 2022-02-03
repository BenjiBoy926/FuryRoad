using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

using TMPro;

public class NetworkRaceResults : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Number of seconds before the results stop displaying and players are thrown into another race")]
    private int secondsUntilNextRace = 10;
    [SerializeField]
    [Tooltip("Text that displays the current state of the countdown")]
    private TextMeshProUGUI countdownText;

    [SerializeField]
    [Tooltip("Reference to the button that leaves the room")]
    private NetworkLeaveRoomButton leaveButton;
    [SerializeField]
    [Tooltip("GUI used to display race results")]
    private RaceResultWidget widgetPrefab;
    [SerializeField]
    [Tooltip("Transform component to instantiate widgets into")]
    private Transform widgetParent;

    Coroutine countdownRoutine;

    public static List<int> ranking = new List<int>();

    #region Monobehaviour Messages
    private void Start()
    {
        Debug.Log($"Ranking:\n- Player {string.Join("\n- Player ", NetworkRaceResults.ranking)}");

        for (int i = 0; i < ranking.Count; i++)
        {
            RaceResultWidget clone = Instantiate(widgetPrefab, widgetParent);
            clone.Setup(ranking[i], i + 1);
        }

        // When we leave the room, stop the countdown
        leaveButton.onLeave.AddListener(() => StopCoroutine(countdownRoutine));

        // Start the countdown now
        countdownRoutine = StartCoroutine(NextRaceCountdownRoutine());
    }
    #endregion

    #region Public Methods
    public static void LoadResults(IReadOnlyList<DrivingManager> ranking)
    {
        // Get a list of the actor numbers of the players controlling each car
        NetworkRaceResults.ranking = ranking
            .Select(driver => driver.driverNumber.Invoke())
            .ToList();
        Debug.Log($"Ranking:\n- Player {string.Join("\n- Player ", NetworkRaceResults.ranking)}");

        if(PhotonNetwork.IsMasterClient)
        {
            NetworkManager.settings.resultsScene.NetworkLoad();
        }
    }
    #endregion

    #region Private Methods
    private IEnumerator NextRaceCountdownRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        for(int i = secondsUntilNextRace; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return wait;
        }

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        // Only the master client loads the level for all other clients
        if(PhotonNetwork.IsMasterClient)
        {
            // If the room is full, load the racing scene again
            if (PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                NetworkManager.settings.raceScene.NetworkLoad();
            }
            // If the room is not full, go back to the lobby
            else NetworkManager.settings.lobbyScene.NetworkLoad();
        }
    }
    #endregion
}
