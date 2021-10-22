using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainModule : DrivingModule, ITopSpeedModifier
{
    #region Private Editor Fields
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag of the object considered to be 'offroad' terrain")]
    private string offroadTag = "Offroad";
    [SerializeField]
    [Tooltip("Top speed adjustment while the vehicle is driving offroad")]
    private float offroadModifier;
    #endregion

    #region Private Fields
    // True if the vehicle is currently off the road
    private bool isOffroad;
    #endregion

    #region Public Properties
    public float modifier => offroadModifier;
    public bool applyModifier => isOffroad;
    #endregion

    #region Monobehaviour Messages
    public void FixedUpdate()
    {
        Collider groundCollider = m_Manager.groundingModule.hit.collider;
        if(groundCollider)
        {
            isOffroad = groundCollider.CompareTag(offroadTag);
        }
    }
    #endregion
}
