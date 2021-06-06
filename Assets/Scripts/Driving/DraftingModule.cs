using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DraftingModule : ITopSpeedModifier
{
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag of the player object to check if we are behind to enable drafting")]
    private string m_PlayerTag = "Player";
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

    public bool draftActive { get; private set; }
    public float modifier => m_TopSpeedModifier;
    public bool applyModifier => draftActive;

    // We had to rename this because of a script error,
    // otherwise we would name this "FixedUpdate"
    public void FixedUpdate(Rigidbody rb, Vector3 heading)
    {
        Ray ray = new Ray(rb.position, heading);

        // Cast a ray forward and see if we hit anyone
        bool gotHit = Physics.Raycast(ray, out RaycastHit hit, m_DraftDistance, m_PlayerLayer, QueryTriggerInteraction.Collide);

        // Check if the draft is active by checking if the raycast got a hit on an object with the correct tag
        draftActive = gotHit && hit.collider.CompareTag(m_PlayerTag);

        if(draftActive)
        {
            rb.AddForce(heading * m_DraftStrength * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
