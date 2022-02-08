using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RacingLapData
{
    #region Public Properties
    public RacingCheckpoint CurrentCheckpoint => currentCheckpoint;
    public int CompletedLaps => completedLaps;
    public int CurrentLap => completedLaps + 1;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the current checkpoint for the player")]
    private RacingCheckpoint currentCheckpoint;
    [SerializeField]
    [Tooltip("Number of laps that the player has completed")]
    private int completedLaps;
    #endregion

    #region Constructors
    public RacingLapData(RacingLapData other) : this(other.currentCheckpoint, other.completedLaps) { }
    public RacingLapData(RacingCheckpoint currentCheckpoint, int currentLap)
    {
        this.currentCheckpoint = currentCheckpoint;
        this.completedLaps = currentLap;
    }
    public RacingLapData(RacingCheckpoint currentCheckpoint) : this(currentCheckpoint, 0) { }
    public RacingLapData() : this(null, 0) { }
    #endregion

    #region Public Methods
    public RacingLapData CheckpointPassed(RacingCheckpoint checkpointPassed, int firstCheckpointOrder, int lastCheckpointOrder)
    {
        // If we have a current checkpoint we need to do some work to check it against the checkpoint just passed
        if (currentCheckpoint)
        {
            // If the checkpoint passed is after the current checkpoint that return the racing lap data with the new checkpoint
            if (checkpointPassed.Order > currentCheckpoint.Order) return new RacingLapData(checkpointPassed, completedLaps);
            // If the checkpoint passed is the first checkpoint and the current checkpoint is the last checkpoint,
            // it means we just finished a lap!
            else if (checkpointPassed.Order == firstCheckpointOrder && currentCheckpoint.Order == lastCheckpointOrder)
            {
                // Return the racing data with the checkpoitn passed and update the lap by 1
                return new RacingLapData(checkpointPassed, completedLaps + 1);
            }
            // This means we passed a checkpoint before the current one, so do not update the current checkpoint
            else return new RacingLapData(this);
        }
        // If the current checkpoint is null we assume that they are on their first lap
        else return new RacingLapData(checkpointPassed, 0);
    }
    #endregion

    #region Operator Overloads
    public static bool operator <=(RacingLapData a, RacingLapData b)
    {
        /* 1. Compare lap
         * 2. If laps are equal, compare checkpoint order
         */
        if (a.CompletedLaps < b.CompletedLaps)
        {
            return true;
        }
        else if (a.CompletedLaps > b.CompletedLaps)
        {
            return false;
        }
        else // Equal completed laps
        {
            if (a.CurrentCheckpoint.Order <= b.CurrentCheckpoint.Order)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public static bool operator >=(RacingLapData a, RacingLapData b)
    {
        if (a.CompletedLaps < b.CompletedLaps)
        {
            return false;
        }
        else if (a.CompletedLaps > b.CompletedLaps)
        {
            return true;
        }
        else // Equal completed laps
        {
            if (a.CurrentCheckpoint.Order < b.CurrentCheckpoint.Order)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    #endregion
}
