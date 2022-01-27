using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class RacingLapUI : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Text used to display the player's current lap")]
    private TextMeshProUGUI currentLapText;
    [SerializeField]
    [Tooltip("Size that the current lap text punches out to")]
    private float punchSize = 2f;
    [SerializeField]
    [Tooltip("Time it takes for the current lap text punch animation to complete")]
    private float punchTime = 0.5f;

    [Space]

    [SerializeField]
    [Tooltip("Text that creates a notification when the player passes a new lap")]
    private TextMeshProUGUI newLapText;
    [SerializeField]
    [Tooltip("Time it takes for the new lap text to grow into existence")]
    private float growTime = 3f;
    [SerializeField]
    [Tooltip("Ending size of the new lap text")]
    private float endingSize = 2f;
    [SerializeField]
    [Tooltip("Starting anchor position of the new lap text")]
    private Vector2 anchorStart;
    [SerializeField]
    [Tooltip("Ending anchor position of the new lap text")]
    private Vector2 anchorEnd;
    [SerializeField]
    [Tooltip("Time it takes for the new lap text to move into view")]
    private float moveInTime = 0.2f;
    [SerializeField]
    [Tooltip("Time it takes for the new lap text to move out of view")]
    private float moveOutTime = 1f;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        // Run the base start method
        base.Start();

        // Current lap is not set yet
        currentLapText.text = "--";
        // Initially, new lap text is invisible
        newLapText.enabled = false;

        // Listen for new lap event
        manager.NewLapEvent.AddListener(OnNewLapPassed);
    }
    #endregion

    #region Event Listeners
    private void OnNewLapPassed(RacingLapData lap)
    {
        // If the game object is not active then simply ignore it
        if (gameObject.activeInHierarchy)
        {
            // Get the racing manager
            RacingManager racingManager = FindObjectOfType<RacingManager>();

            // Update the text if we got a racing manager
            // and we are not past the final lap
            if (racingManager && lap.CurrentLap <= racingManager.TotalLaps)
            {
                // Update current lap text
                currentLapText.text = $"Lap {lap.CurrentLap}/{racingManager.TotalLaps}";

                // Only punch the size for laps after lap 1
                if (lap.CurrentLap > 1)
                {
                    currentLapText.rectTransform.DOComplete();
                    currentLapText.rectTransform.DOPunchScale(Vector3.one * punchSize, punchTime, vibrato: 0, elasticity: 0);

                    // Set the text of the new lap text
                    if (lap.CurrentLap < racingManager.TotalLaps)
                    {
                        newLapText.text = $"Lap {lap.CurrentLap}!";
                    }
                    else newLapText.text = $"Final Lap!!!";

                    // Start the coroutine that animates the new lap text
                    StartCoroutine(NewLapTextAnimation());
                }
            }
        }
    }
    #endregion

    #region Private Methods
    private IEnumerator NewLapTextAnimation()
    {
        // Set the starting position and size of the text
        newLapText.enabled = true;
        newLapText.rectTransform.anchoredPosition = anchorStart;
        newLapText.rectTransform.localScale = Vector3.one;

        // Start the scaling time
        newLapText.rectTransform.DOComplete();
        newLapText.rectTransform
            .DOAnchorPos(anchorEnd, moveInTime)
            .SetEase(Ease.OutBack);
        yield return newLapText.rectTransform
            .DOScale(endingSize, growTime)
            .SetEase(Ease.Linear)
            .WaitForCompletion();

        // Move the text out of view
        yield return newLapText.rectTransform
            .DOAnchorPos(anchorStart, moveOutTime)
            .SetEase(Ease.InBack)
            .WaitForCompletion();
        newLapText.enabled = false;
    }
    #endregion
}
