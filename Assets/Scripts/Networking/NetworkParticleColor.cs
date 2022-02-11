using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkParticleColor : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the particle system to change color for")]
    private ParticleSystem particles;
    [SerializeField]
    [Tooltip("Photon view to use to check what the color of the particle should be")]
    private PhotonView photonView;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        ParticleSystem.TrailModule trails = particles.trails;
        ParticleSystem.MinMaxGradient colorOverTrail = trails.colorOverTrail;
        Gradient gradient = colorOverTrail.gradient;

        // Get color keys and index of latest color key
        GradientColorKey[] colorKeys = gradient.colorKeys;
        int indexOfLatest = 0;

        for (int i = 1; i < colorKeys.Length; i++)
        {
            // If this color key is later than the latest color key,
            // then set the latest index to this one
            if (colorKeys[i].time > colorKeys[indexOfLatest].time)
            {
                indexOfLatest = i;
            }
        }

        // If the photon view is mine them make the end of the trail green
        if (photonView.IsMine)
        {
            colorKeys[indexOfLatest].color = Color.green;
        }
        // If the photon view is someone else's then make the end of the trail red
        else colorKeys[indexOfLatest].color = Color.red;

        // Set the color over trail for the module
        gradient.colorKeys = colorKeys;
        colorOverTrail.gradient = gradient;
        trails.colorOverTrail = colorOverTrail;
    }
    #endregion
}
