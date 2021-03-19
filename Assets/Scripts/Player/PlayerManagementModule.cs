﻿using System.Collections;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class PlayerManagementModule: MonoBehaviour, IPunInstantiateMagicCallback
{
    private PlayerMovementDriver3D movementDriver;

    public int localActorNumber
    {
        get
        {
            return PhotonNetwork.PlayerList.First(x => (PlayerManagementModule)x.TagObject == this).ActorNumber;
        }
    }

    private void Awake()
    {
        movementDriver = GetComponent<PlayerMovementDriver3D>();
    }

    public void EnableControl(bool active)
    {
        movementDriver.enabled = active;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("Player #" + PhotonNetwork.LocalPlayer.ActorNumber +
            " received OnPhotonInstantiate callback from Player #" +
            info.Sender.ActorNumber);
        info.Sender.TagObject = NetworkHelper.GetPlayerManager(gameObject);
        //DontDestroyOnLoad(gameObject);
    }

    //private void OnDestroy()
    //{
    //    PhotonNetwork.LeaveRoom();
    //}

    //private void OnApplicationQuit()
    //{
    //    PhotonNetwork.LeaveRoom();
    //}
}