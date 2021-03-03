using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraSetupModule))]
public class CameraNetworkSetupDriver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CameraSetupModule>().Setup(NetworkHelper.localPlayerManager.transform);
    }
}
