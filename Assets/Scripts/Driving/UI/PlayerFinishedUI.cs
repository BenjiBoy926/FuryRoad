using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerFinishedUI : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the player manager to invoke the race finished event")]
    private PlayerManager manager;
    [SerializeField]
    [Tooltip("Reference to the game object to enable when the race is finished")]
    private GameObject banner;
    [SerializeField]
    [Tooltip("Text to show what place the player got")]
    private TextMeshProUGUI textDisplay;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        manager.PlayerFinishedEvent.AddListener(UpdateDisplay);
    }
    #endregion

    #region Private Methods
    private void UpdateDisplay(int rank)
    {
        banner.SetActive(true);
        textDisplay.text = StringUtilities.Ordinal(rank + 1) + " Place!";
    }
    #endregion
}
