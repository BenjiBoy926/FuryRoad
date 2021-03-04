using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementModule3D))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Distance back from the target for the camera")]
    private float backDistance;
    [SerializeField]
    [Tooltip("Distance up from the target for the camera")]
    private float lift;

    private new Transform camera;
    private MovementModule3D movement;
    private Vector3 offset = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        movement = GetComponent<MovementModule3D>();
    }

    // Update is called once per frame
    void Update()
    {
        offset = (movement.heading * (-backDistance)) + (Vector3.up * lift);
        camera.position = transform.position + offset;
        camera.rotation = Quaternion.LookRotation(movement.heading, Vector3.up);
    }
}
