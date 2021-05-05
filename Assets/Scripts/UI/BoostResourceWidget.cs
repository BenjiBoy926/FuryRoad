using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostResourceWidget : MonoBehaviour, System.IComparable<BoostResourceWidget>
{
    [SerializeField]
    [Tooltip("Reference to the image that displays when we have a boost resource available")]
    private Image image;
    [SerializeField]
    [Tooltip("Color of the resource when available")]
    private Color available;
    [SerializeField]
    [Tooltip("Color of the resource when not available")]
    private Color unavailable;

    public void SetActive(bool active)
    {
        if (active) image.color = available;
        else image.color = unavailable;
    }

    // Sort from widget lowest to widget highest
    public int CompareTo(BoostResourceWidget other)
    {
        return (int)(transform.position.y - other.transform.position.y);
    }
}
