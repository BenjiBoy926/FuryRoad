using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

[System.Serializable]
public class NetworkRaceCount
{
    [SerializeField]
    [Tooltip("Game object to activate on this count")]
    private GameObject gameObject;
    [SerializeField]
    [Tooltip("Text to display on this count")]
    private string text;

    public NetworkRaceCount(GameObject gameObject, string text)
    {
        this.gameObject = gameObject;
        this.text = text;
    }

    public void SetActive(bool active, TextMeshProUGUI mesh)
    {
        gameObject.SetActive(active);
        if (active) mesh.text = text;
    }
}
