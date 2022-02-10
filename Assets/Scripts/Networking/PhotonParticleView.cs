using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(ParticleSystem))]
public class PhotonParticleView : MonoBehaviour, IPunObservable
{
    #region Private Properties
    private ParticleSystem Particles
    {
        get
        {
            if (!particles) particles = GetComponent<ParticleSystem>();
            return particles;
        }
    }
    #endregion

    #region Private Fields
    private ParticleSystem particles;
    #endregion

    #region IPunObservable Implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        bool currentIsPlaying = Particles.isPlaying;
        stream.Serialize(ref currentIsPlaying);

        // If particles should be playing and aren't then play them
        if (currentIsPlaying && !Particles.isPlaying) Particles.Play();
        // If particles shouldn't be playing and are then stop them
        else if (!currentIsPlaying && Particles.isPlaying) Particles.Stop();
    }
    #endregion
}
