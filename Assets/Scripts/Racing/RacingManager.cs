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
    public class PlayerManagerRacingCheckpointEvent : UnityEvent<PlayerManager, RacingCheckpoint> { }
    #endregion

    #region Public Properties
    public IReadOnlyDictionary<PlayerManager, RacingLapData> PlayerLapData => playerLapData;
    public IReadOnlyList<PlayerManager> Ranking => ranking;
    public UnityEvent<PlayerManager, RacingCheckpoint> CheckpointPassedEvent => checkpointPassedEvent;
    public UnityEvent AllRacersFinishedEvent => allRacersFinishedEvent;
    #endregion

    #region Private Properties
    private RacingCheckpoint[] Checkpoints => FindObjectsOfType<RacingCheckpoint>();
    private PlayerManager[] Racers => FindObjectsOfType<PlayerManager>();
    private int FirstCheckpointOrder => Checkpoints.Min(checkpoint => checkpoint.Order);
    private int LastCheckpointOrder => Checkpoints.Max(checkpoint => checkpoint.Order);
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
    private PlayerManagerRacingCheckpointEvent checkpointPassedEvent;
    [SerializeField]
    [Tooltip("Event invoked when all players finish the race")]
    private UnityEvent allRacersFinishedEvent;
    #endregion

    #region Private Fields
    // Maps each player to their current checkpoint
    private Dictionary<PlayerManager, RacingLapData> playerLapData = new Dictionary<PlayerManager, RacingLapData>();
    // Current ranking of players, where the earliest player in the list got first place
    private List<PlayerManager> ranking = new List<PlayerManager>();
    private bool finishedEventInvoked = false;
    #endregion

    #region Monobehaviour Messages
    // Start is called before the first frame update
    void Start()
    {
        PlayerManager[] allPlayers = FindObjectsOfType<PlayerManager>();

        // Add a kvp for each player in the list
        foreach(PlayerManager player in allPlayers)
        {
            if (!playerLapData.ContainsKey(player))
            {
                playerLapData.Add(player, new RacingLapData());
                player.OnNewLap(playerLapData[player]);
            }
        }

        // At the start, the race finished event has not been invoked yet
        finishedEventInvoked = false;

        // Play the racing music
        racingMusicSource.clip = racingMusic;
        racingMusicSource.Play();
    }
    #endregion

    #region Public Methods
    public void OnCheckpointPassed(PlayerManager player, RacingCheckpoint checkpoint)
    {
        if (playerLapData.ContainsKey(player))
        {
            playerLapData[player] = playerLapData[player].CheckpointPassed(checkpoint, FirstCheckpointOrder, LastCheckpointOrder, out bool newLap);

            if (newLap)
            {
                // Notify the player that they just passed a new lap
                player.OnNewLap(playerLapData[player]);

                // If the player passed the final lap and they are not in the ranking yet then add them to the ranking
                if(playerLapData[player].CurrentLap >= totalLaps && !ranking.Contains(player))
                {
                    ranking.Add(player);

                    // Notify the player that they just finished the race
                    player.OnRaceFinished(ranking.Count - 1);

                    // When the ranking count exceeds the racer count then invoke all racers finished event
                    if (ranking.Count >= Racers.Length) OnAllRacersFinished();
                }
            }
        }
        else playerLapData.Add(player, new RacingLapData(checkpoint));

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
