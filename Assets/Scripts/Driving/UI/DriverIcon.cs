using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DriverIcon : MonoBehaviour
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
        text.text = driver.ID;
    }
    #endregion
}
