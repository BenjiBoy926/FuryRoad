using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementUIManager : MonoBehaviour
{
    public FinishLine Raceranking;
    [SerializeField]
    [Tooltip("Text that displays the placement of racer")]
    private Text text;

    public void UpdateUI(int placement)
    {
        text.text = placement.ToString();
    }
}
