using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Describe a widget that displays the result for a single racer
public class RaceResultWidget : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Color of the widget when it describes the local racer")]
    private Color myColor;
    [SerializeField]
    [Tooltip("Color of the widget when it describes a competing racer")]
    private Color otherColor;

    [SerializeField]
    [Tooltip("Text to display the rank of the racer")]
    private Text rankText;
    [SerializeField]
    [Tooltip("Text to display the racer's ID")]
    private Text idText;

    // Actor number for the current racer
    private int actorNumber;
    // Rank number for the current racer
    private int rank;
    // Reference to the image that displays the border
    private Image border;

    public void Setup(int actorNumber, int rank)
    {
        this.actorNumber = actorNumber;
        this.rank = rank;

        if(border == null)
        {
            border = GetComponent<Image>();
        }

        SetText();
        SetColor();
    }

    private void SetText()
    {
        rankText.text = RaceHelper.OrdinalString(rank);
        idText.text = "Player #" + actorNumber;
    }

    private void SetColor()
    {
        Color newColor;

        if(PlayerManager.local.index == actorNumber)
        {
            newColor = myColor;
        }
        else
        {
            newColor = otherColor;
        }

        border.color = newColor;
        rankText.color = newColor;
        idText.color = newColor;
    }
}
