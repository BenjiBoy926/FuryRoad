using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceWidget : MonoBehaviour, System.IComparable<ResourceWidget>
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the image that displays when we have a boost resource available")]
    private Graphic[] graphics;
    [SerializeField]
    [Tooltip("Color of the resource when available")]
    private Color available;
    [SerializeField]
    [Tooltip("Color of the resource when not available")]
    private Color unavailable;
    #endregion

    #region Public Methods
    public void SetActive(bool active)
    {
        if (active) SetColor(available);
        else SetColor(unavailable);
    }
    // Sort from widget lowest to widget highest
    public int CompareTo(ResourceWidget other)
    {
        return (int)(transform.position.y - other.transform.position.y);
    }
    #endregion

    #region Private Methods
    private void SetColor(Color color)
    {
        foreach (Graphic g in graphics)
        {
            g.color = color;
        }
    }
    #endregion
}
