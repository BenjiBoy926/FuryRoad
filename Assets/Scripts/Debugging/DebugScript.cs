using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    private void OnDestroy()
    {
        Debug.Log(gameObject.name + " has been DESTROYED?!");
    }
}
