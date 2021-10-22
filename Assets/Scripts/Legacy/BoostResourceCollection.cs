using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostResourceCollection : MonoBehaviour
{
    [SerializeField]
    [Tooltip("List of widgets used to display when we have a boost resource")]
    private List<BoostResourceWidget> widgets;

    public void UpdateWidgets(int boosts)
    {
        widgets.Sort();
        for(int i = 0; i < widgets.Count; i++)
        {
            widgets[i].SetActive(i < boosts);
        }
    }
}
