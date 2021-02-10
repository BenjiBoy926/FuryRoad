using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GroundingModule : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Physics layer to check for ground collisions")]
    private LayerMask groundMask;

    // Collider on the car
    private new BoxCollider collider;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    public bool Grounded()
    {
        // The ray that is cast at each corner of the box collider
        Ray ray = new Ray(Vector3.zero, transform.up * -1);

        // Position of the center of the bottom face of the box collider in world space
        Vector3 bottom = collider.center - (transform.up * (collider.size.y / 2f)) + transform.position + transform.up * 0.1f;
        // Middle bottom of the forward edge of the box collider
        Vector3 forward = bottom + (transform.forward * (collider.size.z / 2f));
        // Middle bottom of the backward edge of the box collider
        Vector3 back = bottom - (transform.forward * (collider.size.z / 2f));

        // Length of the raycast
        float length = 0.2f;

        // Cast the ray at the front-right-bottom corner
        ray.origin = forward + (transform.right * (collider.size.x / 2f));
        Debug.DrawRay(ray.origin, ray.direction.normalized * length);
        // We check to see if each raycast hits after each raycast so we can terminate early without doing every raycast
        if (Physics.Raycast(ray, length, groundMask))
        {
            return true;
        }

        // Cast the ray at the front-left-bottom corner
        ray.origin = forward - (transform.right * (collider.size.x / 2f));
        Debug.DrawRay(ray.origin, ray.direction.normalized * length);
        if (Physics.Raycast(ray, length, groundMask))
        {
            return true;
        }

        // Cast the ray at the back-right-bottom corner
        ray.origin = back + (transform.right * (collider.size.x / 2f));
        Debug.DrawRay(ray.origin, ray.direction.normalized * length);
        if (Physics.Raycast(ray, length, groundMask))
        {
            return true;
        }

        // Cast a ray at the back-left-bottom corner
        ray.origin = back - (transform.right * (collider.size.x / 2f));
        Debug.DrawRay(ray.origin, ray.direction.normalized * length);
        if (Physics.Raycast(ray, length, groundMask))
        {
            return true;
        }

        return false;
    }
}