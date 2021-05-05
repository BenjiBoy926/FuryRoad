using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    private void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = Random.insideUnitCircle * 20f;
    }

    private void OnDestroy()
    {
        Debug.Log("I've been DESTROYED?!");
    }
}
