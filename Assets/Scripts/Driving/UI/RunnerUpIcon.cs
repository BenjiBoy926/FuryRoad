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
        // NOTE: this may not be defined if the driving manager is not yet set up
        text.text = driver.ID;
    }
    public void SetAnchoredPosition(Vector2 anchor)
    {
        rectTransform.anchoredPosition = anchor;

        // Make the transform point at the anchor point
        PointToAnchor();
    }
    #endregion

    #region Private Fields
    private void PointToAnchor()
    {
        // Set the anchor position
        Vector2 anchor = rectTransform.anchoredPosition;

        // Make the transform point at the anchor point
        float angle = Mathf.Atan(anchor.x / anchor.y) * Mathf.Rad2Deg * -1f;
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
    #endregion
}
