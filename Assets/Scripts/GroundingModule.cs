using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundingModule : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Physics layer to check for ground collisions")]
    private LayerMask groundMask;
    [SerializeField]
    [Tooltip("The colliders for the wheels of the cart")]
    private List<Collider> wheels;

    public bool Grounded()
    {
        Ray ray = new Ray(Vector3.zero, transform.up * -1f);
        bool grnd = false;

        for (int i = 0; i < wheels.Count; i++)
        {
            ray.origin = wheels[i].bounds.center + (wheels[i].bounds.extents.y * transform.up * -1);
            grnd |= Physics.Raycast(ray, 0.1f);

            Debug.DrawRay(ray.origin, ray.direction);
        }

        return grnd;
    }
}