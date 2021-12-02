using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

[System.Serializable]
public class NetworkRaceFinishUI
{
    [SerializeField]
    [Tooltip("Root object on the finish UI")]
    private GameObject root;
    public void Start()
    {
        SetActive(false);
    }
    public void SetActive(bool active)
    {
        root.SetActive(active);
    }
}
