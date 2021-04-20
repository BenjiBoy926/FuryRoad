using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GroundingModule : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Distance of the raycast down from the collider")]
    private float raycastDistance;
    [SerializeField]
    [Tooltip("Physics layer to check for ground collisions")]
    private LayerMask groundMask;

    // Collider on the car
    private Collider _collider;
    // True if the racast has already updated this frame
    private bool raycastQueryUpdated = false;
    // True if the module is currently grounded
    private bool _grounded = false;
    // Information about what the raycast hit
    private RaycastHit _hit = new RaycastHit() { normal = Vector3.up };

    private new Collider collider
    {
        get
        {
            if(_collider == null)
            {
                _collider = GetComponent<Collider>();
            }
            return _collider;
        }
    }
    public bool grounded
    {
        get
        {
            if(!raycastQueryUpdated)
            {
                UpdateRaycastQuery();
            }
            return _grounded;
        }
    }
    public RaycastHit hit
    {
        get
        {
            if(!raycastQueryUpdated)
            {
                UpdateRaycastQuery();
            }
            return _hit;
        }
    }
    // Normal with the ground.  If we are not hitting anything, then the normal is straight up
    public Vector3 groundNormal
    {
        get
        {
            if (grounded) return hit.normal;
            else return Vector3.up;
        }
    }

    // At the end of the frame, set raycast query updated to false
    private void LateUpdate()
    {
        raycastQueryUpdated = false;
    }

    private void UpdateRaycastQuery()
    {
        float margin = 0.1f;
        Vector3 origin = collider.bounds.center + Vector3.down * (collider.bounds.extents.y - margin);
        Vector3 direction = Vector3.down;

        // The ray that is cast down from the bottom of the collider
        Ray ray = new Ray(origin, direction);

        // Cast the ray, and store the result in "grounded" and "hit"
        RaycastHit tempHit;
        _grounded = Physics.Raycast(ray, out tempHit, raycastDistance + margin, groundMask);

        // Only update the hit if the value changed, otherwise we want to keep the most recent value in tact
        if (_grounded) _hit = tempHit;
    }
}