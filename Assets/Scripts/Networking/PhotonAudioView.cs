using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(AudioSource))]
public class PhotonAudioView : MonoBehaviour, IPunObservable
{
    #region Private Properties
    private AudioSource Source
    {
        get
        {
            if (!source) source = GetComponent<AudioSource>();
            return source;
        }
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Whether to sync the volume of the source")]
    private bool syncVolume;
    [SerializeField]
    [Tooltip("Whether to sync the pitch of the source")]
    private bool syncPitch;
    #endregion

    #region Private Fields
    private AudioSource source;
    #endregion

    #region IPunObservable Overrides
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Serialize the is playing boolean
        bool currentIsPlaying = Source.isPlaying;

        // Write audio source properties
        if (stream.IsWriting)
        {
            stream.SendNext(Source.isPlaying);
            if (syncVolume) stream.SendNext(source.volume);
            if (syncPitch) stream.SendNext(source.pitch);
        }
        // Read audio source properties
        else
        {
            currentIsPlaying = (bool)stream.ReceiveNext();
            if (syncVolume) source.volume = (float)stream.ReceiveNext();
            if (syncPitch) source.pitch = (float)stream.ReceiveNext();
        }

        // If we should be playing but aren't then start playing
        if (currentIsPlaying && !Source.isPlaying) Source.Play();
        // If we shouldn't be playing but are then stop playing
        else if (!currentIsPlaying && Source.isPlaying) Source.Stop();
    }
    #endregion
}
