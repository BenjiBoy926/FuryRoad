using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Setup the player for networked play by enabling/disabling
/// game objects and components on the player based on whether
/// the given photon view is mine
/// </summary>
public class NetworkPlayerSetup : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the photon view to check for ownership")]
    private PhotonView view;
    [SerializeField]
    [Tooltip("List of game objects to enable/disable depending on ownership of the photon view provided")]
    private GameObject[] networkSensitiveObjects;
    [SerializeField]
    [Tooltip("List of components to enable/disable depending on ownership of the photon view provided")]
    private MonoBehaviour[] networkSensitiveBehaviours;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        foreach(GameObject obj in networkSensitiveObjects)
        {
            obj.SetActive(view.IsMine);
        }
        foreach(MonoBehaviour behaviour in networkSensitiveBehaviours)
        {
            behaviour.enabled = view.IsMine;
        }
    }
    #endregion
}
