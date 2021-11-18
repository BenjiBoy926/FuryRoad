using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RacingLapData
{
    #region Public Properties
    public RacingCheckpoint CurrentCheckpoint => currentCheckpoint;
    public int CurrentLap => currentLap;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the current checkpoint for the player")]
    private RacingCheckpoint currentCheckpoint;
    [SerializeField]
    [Tooltip("Current lap that the player is on")]
    private int currentLap;
    #endregion

    #region Constructors
    public RacingLapData(RacingLapData other) : this(other.currentCheckpoint, other.currentLap) { }
    public RacingLapData(RacingCheckpoint currentCheckpoint, int currentLap)
    {
        this.currentCheckpoint = currentCheckpoint;
        this.currentLap = currentLap;
    }
    public RacingLapData(RacingCheckpoint currentCheckpoint) : this(currentCheckpoint, 0) { }
    public RacingLapData() : this(null, 0) { }
    #endregion

    #region Public Methods
    public RacingLapData CheckpointPassed(RacingCheckpoint checkpointPassed, int firstCheckpointOrder, int lastCheckpointOrder, out bool newLap)
    {
        // By default, we did not pass a new lap
        newLap = false;

        // If we have a current checkpoint we need to do some work to check it against the checkpoint just passed
        if (currentCheckpoint)
        {
            // If the checkpoint passed is after the current checkpoint that return the racing lap data with the new checkpoint
            if (checkpointPassed.Order > currentCheckpoint.Order) return new RacingLapData(checkpointPassed, currentLap);
            // If the checkpoint passed is the first checkpoint and the current checkpoint is the last checkpoint,
            // it means we just finished a lap!
            else if (checkpointPassed.Order == firstCheckpointOrder && currentCheckpoint.Order == lastCheckpointOrder)
            {
                // Notify the player that they just passed a lap
                newLap = true;
                // Return the racing data with the checkpoitn passed and update the lap by 1
                return new RacingLapData(checkpointPassed, currentLap + 1);
            }
            // This means we passed a checkpoint before the current one, so do not update the current checkpoint
            else return new RacingLapData(this);
        }
        // If the current checkpoint is null we assume that they are on their first lap
        else return new RacingLapData(checkpointPassed, 0);
    }
    #endregion
}
