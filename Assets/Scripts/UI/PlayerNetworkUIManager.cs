using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworkUIManager : PlayerUIManager
{
    #region Private Properties
    private PhotonView View
    {
        get
        {
            if(!view)
            {
                view = GetComponentInParent<PhotonView>();

                if (!view) throw new MissingComponentException($"{nameof(PlayerNetworkUIManager)}: " +
                     $"expected a component of type '{nameof(PhotonView)}' in this object or one of its parents " +
                     $"but no such component could be found");
            }

            return view;
        }
    }
    #endregion

    #region Private Fields
    private PhotonView view;
    #endregion

    #region Public Methods
    public override void UpdateBoostResourceUI(float boostPower, int boosts)
    {
        if(View.IsMine)
        {
            base.UpdateBoostResourceUI(boostPower, boosts);
        }
    }
    public override void UpdatePlacementUI(int placement)
    {
        if(View.IsMine)
        {
            base.UpdatePlacementUI(placement);
        }
    }
    public override void UpdateSpeedUI(float speed)
    {
        if(View.IsMine)
        {
            base.UpdateSpeedUI(speed);
        }
    }
    #endregion
}
