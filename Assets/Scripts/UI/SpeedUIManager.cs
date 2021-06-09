using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUIManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Text that displays the speed of the car")]
    private Text text;

    public void UpdateUI(Vector3 velocity, Vector3 groundNormal)
    {
        Vector3 drivingComponent = Vector3.ProjectOnPlane(velocity, groundNormal);
        float speed = Mathf.Round(drivingComponent.magnitude);
        text.text = speed.ToString() + " mps";
    }
}
