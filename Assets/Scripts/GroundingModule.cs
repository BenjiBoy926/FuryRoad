using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GroundingModule : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Physics layer to check for ground collisions")]
    private LayerMask groundMask;

    // Collider on the car
    private new Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    public bool Grounded()
    {
        bool grnd = false;
        Vector3 originOffset = Vector3.up * 0.1f;
        float length = 0.2f;

        // Cast the ray at the min position
        Ray ray = new Ray(collider.bounds.min + originOffset, transform.up * -1f);
        grnd |= Physics.Raycast(ray, length, groundMask);
        Debug.DrawRay(ray.origin, ray.direction);

        // We check to see if each raycast hits after each raycast so we can terminate early without doing every raycast
        if (grnd) return grnd;

        // Cast the ray to right of the min corner
        ray.origin = collider.bounds.min + Vector3.right * collider.bounds.size.x + originOffset;
        grnd |= Physics.Raycast(ray, length, groundMask);
        Debug.DrawRay(ray.origin, ray.direction);
        if (grnd) return grnd;

        // Cast the ray in front of the min corner
        ray.origin = collider.bounds.min + Vector3.forward * collider.bounds.size.z + originOffset;
        grnd |= Physics.Raycast(ray, length, groundMask);
        Debug.DrawRay(ray.origin, ray.direction);
        if (grnd) return grnd;

        // Cast a ray in front and to the right of the min corner
        ray.origin = collider.bounds.min + Vector3.right * collider.bounds.size.x  + Vector3.forward * collider.bounds.size.z + originOffset;
        grnd |= Physics.Raycast(ray, length, groundMask);
        Debug.DrawRay(ray.origin, ray.direction);

        return grnd;
    }
}