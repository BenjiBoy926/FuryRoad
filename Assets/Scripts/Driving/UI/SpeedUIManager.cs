using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUIManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Text that displays the speed of the car")]
    private Text text;

    public void UpdateUI(float speed)
    {
        text.text = Mathf.Round(speed) + " mps";
    }
}
