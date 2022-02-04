using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DraftingModule : DrivingModule, ITopSpeedModifier
{
    #region Public Properties
    public bool draftActive { get; private set; }
    public float modifier => m_TopSpeedModifier;
    public bool applyModifier => draftActive;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Layer of the player objects")]
    private LayerMask m_PlayerLayer;
    [SerializeField]
    [Tooltip("Strength of the force that pushes the car when drafting behind another car")]
    private float m_DraftStrength = 30f;
    [SerializeField]
    [Tooltip("How close the car must be to be caught in a draft")]
    private float m_DraftDistance = 100f;
    [SerializeField]
    [Tooltip("Modifies the top speed of the car while drafting behind another car")]
    private float m_TopSpeedModifier = 1.2f;
    [SerializeField]
    [Tooltip("Particles displayed while the vehicle is drafting")]
    private ParticleSystem particles;
    #endregion

    #region Monobehaviour Messages
    // We had to rename this because of a script error,
    // otherwise we would name this "FixedUpdate"
    private void FixedUpdate()
    {
        Ray ray = new Ray(m_Manager.rigidbody.position, m_Manager.forward);

        // Cast a ray forward and see if we hit anyone
        draftActive = Physics.Raycast(ray, m_DraftDistance, m_PlayerLayer, QueryTriggerInteraction.Collide);

        // If draft is active, force the rigidbody and enable particles
        if (draftActive)
        {
            m_Manager.rigidbody.AddForce(m_Manager.forward * m_DraftStrength * Time.fixedDeltaTime, ForceMode.VelocityChange);
            if (!particles.isPlaying) particles.Play();
        }
        // If not drafting and particles are still playing, stop them
        else if (particles.isPlaying) particles.Stop();
    }
    #endregion
}
