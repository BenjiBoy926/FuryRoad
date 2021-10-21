using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public static class NetworkUtilities
{
    public static GameObject InstantiateLocalOrNetwork(GameObject gameObject, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
    {
        if (PhotonNetwork.IsConnected)
        {
            return PhotonNetwork.Instantiate(gameObject.name, position, rotation, group, data);
        }
        else return Object.Instantiate(gameObject, position, rotation);
    }
    public static TComponent InstantiateLocalOrNetwork<TComponent>(TComponent prefab, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
        where TComponent : Component
    {
        GameObject obj = InstantiateLocalOrNetwork(prefab.gameObject, position, rotation, group, data);

        // Safe to not null check because we know the game object MUST have the component attached,
        // since we instantiated the exact game object it was attached to
        return obj.GetComponent<TComponent>();
    }
    public static void DestroyLocalOrNetwork(GameObject gameObject)
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else Object.Destroy(gameObject);
    }
}
