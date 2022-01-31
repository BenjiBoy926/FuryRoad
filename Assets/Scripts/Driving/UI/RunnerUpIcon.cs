using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RunnerUpIcon : MonoBehaviour
{
    #region Public Properties
    public RectTransform RectTransform => rectTransform;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the rect transform used to place the icon")]
    private RectTransform rectTransform;
    [SerializeField]
    [Tooltip("Text used to display which driver this icon represents")]
    private TextMeshProUGUI text;
    #endregion

    #region Public Methods
    public void DisplayDriver(DrivingManager driver)
    {
        // Try to get a network player on the driver
        NetworkPlayer player = driver.GetComponent<NetworkPlayer>();

        // If the player exists then display the network actor number
        if (player) text.text = $"P{player.photonView.OwnerActorNr}";
        // If the player does not exist use a placeholder
        else text.text = "P0";
    }
    #endregion
}
