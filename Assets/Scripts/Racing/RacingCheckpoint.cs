using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RacingCheckpoint : MonoBehaviour
{
    #region Public Properties
    public int Order => order;
    #endregion

    #region Private Properties
    private RacingManager Manager
    {
        get
        {
            if(!manager)
            {
                manager = FindObjectOfType<RacingManager>();

                // If the manager still cannot be found, and the missing notification is not sent yet, then log the notification
                if(!manager && !managerMissingNotified)
                {
                    Debug.Log($"{nameof(RacingCheckpoint)}: cannot find a racing manager in scene named '{SceneManager.GetActiveScene().name}', " +
                        $"so the checkpoint system will not do anything in this scene", this);
                    managerMissingNotified = true;
                }
            }
            return manager;
        }
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Order that the player is expected to hit this checkpoint")]
    private int order;
    #endregion

    #region Private Fields
    private RacingManager manager;
    private static bool managerMissingNotified = false;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        managerMissingNotified = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        DrivingManager driver = other.GetComponentInParent<DrivingManager>();

        if(driver)
        {
            // Notify the racing manager that a player passed this checkpoint
            if(Manager)
            {
                Manager.OnCheckpointPassed(driver, this);
            }
        }
    }
    private void OnValidate()
    {
        name = "Checkpoint #" + order.ToString();
    }
    #endregion
}
