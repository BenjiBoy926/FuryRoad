using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DraftingModule : MonoBehaviour, ITopSpeedModifier
{
    [SerializeField]
    [TagSelector]
    [Tooltip("Tag of the drafting volume that enables drafting")]
    private string m_DraftVolumeTag = "DraftingVolume";
    [SerializeField]
    [Tooltip("Strength of the force that pushes the car when drafting behind another car")]
    private float m_DraftStrength = 30f;
    [SerializeField]
    [Tooltip("Modifies the top speed of the car while drafting behind another car")]
    private float m_TopSpeedModifier = 1.2f;

    private List<Collider> draftingVolumes = new List<Collider>();

    public bool draftActive => draftingVolumes.Count > 0;
    public float modifier => m_TopSpeedModifier;
    public bool applyModifier => draftActive;

    // We had to rename this because of a script error,
    // otherwise we would name this "FixedUpdate"
    public void ModuleUpdate(Rigidbody rb, Vector3 heading)
    {
        if(draftActive)
        {
            rb.AddForce(heading * m_DraftStrength * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag(m_DraftVolumeTag) && !draftingVolumes.Contains(other))
        {
            draftingVolumes.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        draftingVolumes.Remove(other);
    }
}
