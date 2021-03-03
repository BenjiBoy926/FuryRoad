using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CameraSetupModule))]
public class CameraLocalSetupDriver : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        CameraSetupModule setupModule = GetComponent<CameraSetupModule>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        setupModule.Setup(player.transform);
    }
}
