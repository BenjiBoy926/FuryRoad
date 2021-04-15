using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DriftingModule
{
    [SerializeField]
    [Tooltip("Min-max radii for the vehicle's drifting")]
    private FloatRange radiusRange;

    // Rigidbody with affected velocity for the drift
    private Rigidbody m_Rigidbody;
}
