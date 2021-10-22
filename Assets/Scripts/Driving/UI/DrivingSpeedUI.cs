using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrivingSpeedUI : DrivingModule
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Text that displays the speed of the car")]
    private Text speedText;
    #endregion

    #region Monobehaviour Messages
    private void Update()
    {
        speedText.text = (int)manager.drivingSpeed + " mph";
    }
    #endregion
}
