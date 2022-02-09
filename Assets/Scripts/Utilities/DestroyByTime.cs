using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Amount of time after creating the object to immediately destroy it")]
    private float lifetime = 2f;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    #endregion
}
