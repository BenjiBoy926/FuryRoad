using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class RacingManager : MonoBehaviour
{
    #region Public Typedefs
    [System.Serializable]
    public class DrivingManagerRacingCheckpointEvent : UnityEvent<DrivingManager, RacingCheckpoint> { }
    #endregion

    #region Public Properties
    public int TotalLaps => totalLaps;
    public IReadOnlyDictionary<DrivingManager, RacingLapData> PlayerLapData => playerLapData;
    public IReadOnlyList<DrivingManager> Ranking => ranking;
    public UnityEvent<DrivingManager, RacingCheckpoint> CheckpointPassedEvent => checkpointPassedEvent;
    public UnityEvent AllRacersFinishedEvent => allRacersFinishedEvent;
    #endregion

    #region Private Properties
    private RacingCheckpoint[] Checkpoints => FindObjectsOfType<RacingCheckpoint>();
    private int EarliestCheckpointOrder => Checkpoints.Min(checkpoint => checkpoint.Order);
    private RacingCheckpoint EarliestCheckpoint => System.Array.Find(Checkpoints, check => check.Order == EarliestCheckpointOrder);
    private int LatestCheckpointOrder => Checkpoints.Max(checkpoint => checkpoint.Order);
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Max laps for the player to complete until they finish the race")]
    private int totalLaps = 3;

    [SerializeField]
    [Tooltip("Audio clip to play for the racing soundtrack")]
    private AudioClip racingMusic;
    [SerializeField]
    [Tooltip("Audio source used to play the racing soundtrack")]
    private AudioSource racingMusicSource;

    [SerializeField]
    [Tooltip("Event invoked when a player passes a checkpoint")]
    private DrivingManagerRacingCheckpointEvent checkpointPassedEvent;
    [SerializeField]
    [Tooltip("Event invoked when all players finish the race")]
    private UnityEvent allRacersFinishedEvent;
    #endregion

    #region Private Fields
    // Maps each player to their current checkpoint
    private Dictionary<DrivingManager, RacingLapData> playerLapData = new Dictionary<DrivingManager, RacingLapData>();
    // Current ranking of players, where the earliest player in the list got first place
    private List<DrivingManager> ranking = new List<DrivingManager>();
    private bool finishedEventInvoked = false;
    #endregion

    #region Monobehaviour Messages
    // Start is called before the first frame update
    void Start()
    {
        // Store the earliest checkpoint so it is not re-calculated per driver
        RacingCheckpoint earliest = EarliestCheckpoint;

        // Add the initial lap data for each driver
        foreach (DrivingManager driver in DriverRegistry.Registry)
        {
            RacingLapData lapData = new RacingLapData(earliest, 0);
            playerLapData.Add(driver, lapData);
            driver.OnNewLap(lapData);
        }

        // At the start, the race finished event has not been invoked yet
        finishedEventInvoked = false;

        // Play the racing music
        racingMusicSource.clip = racingMusic;
        racingMusicSource.Play();
    }
    #endregion

    #region Public Methods
    public void OnCheckpointPassed(DrivingManager player, RacingCheckpoint checkpoint)
    {
        if (playerLapData.ContainsKey(player))
        {
            // Update the current lap data associated with the player
            RacingLapData previous = playerLapData[player];
            playerLapData[player] = playerLapData[player].CheckpointPassed(checkpoint, EarliestCheckpointOrder, LatestCheckpointOrder);

            // If player is in last place...
            if (playerLapData.Values.All(lapData => playerLapData[player] <= lapData))
            {
                // do boost
                player.boostingModule.StartEffect();
            }

            if (previous.CompletedLaps != playerLapData[player].CompletedLaps)
            {
                // Notify the player that they just passed a new lap
                player.OnNewLap(playerLapData[player]);

                // If the player passed the final lap and they are not in the ranking yet then add them to the ranking
                if (playerLapData[player].CompletedLaps >= totalLaps && !ranking.Contains(player))
                {
                    ranking.Add(player);

                    // Notify the player that they just finished the race
                    player.OnRaceFinished(ranking.Count - 1);

                    // When the ranking count exceeds the racer count then invoke all racers finished event
                    if (ranking.Count >= DriverRegistry.Registry.Count) OnAllRacersFinished();
                }
            }
        }
        else
        {
            playerLapData.Add(player, new RacingLapData(checkpoint));
            player.OnNewLap(playerLapData[player]);
        }

        // Invoke the checkpoint passed event
        checkpointPassedEvent.Invoke(player, checkpoint);
    }
    public void OnAllRacersFinished()
    {
        // Invoke all racers finished event only if it has not been invoked already
        if(!finishedEventInvoked)
        {
            allRacersFinishedEvent.Invoke();
            finishedEventInvoked = true;
        }
    }
    #endregion
}
