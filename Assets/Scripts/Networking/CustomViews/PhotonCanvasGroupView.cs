using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(CanvasGroup))]
public class PhotonCanvasGroupView : MonoBehaviour, IPunObservable
{
    #region Private Properties
    private CanvasGroup Group
    {
        get
        {
            // Check if there is a group
            if (!group)
            {
                group = GetComponent<CanvasGroup>();

                // If no group was found then throw an exception
                if (!group) throw new MissingComponentException(
                    $"A component of type {nameof(CanvasGroup)} " +
                    $"must be attached to {this}");
            }

            return group;
        }
    }
    #endregion

    #region Private Fields
    private CanvasGroup group;
    #endregion

    #region Interface Implementations
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            Group.alpha = (float)stream.ReceiveNext();
            Group.interactable = (bool)stream.ReceiveNext();
            Group.blocksRaycasts = (bool)stream.ReceiveNext();
            Group.ignoreParentGroups = (bool)stream.ReceiveNext();
        }
        else
        {
            stream.SendNext(Group.alpha);
            stream.SendNext(Group.interactable);
            stream.SendNext(Group.blocksRaycasts);
            stream.SendNext(Group.ignoreParentGroups);
        }
    }
    #endregion
}
