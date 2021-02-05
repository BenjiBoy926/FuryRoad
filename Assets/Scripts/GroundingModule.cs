using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundingModule
{
    [SerializeField]
    [Tooltip("Physics layer to check for ground collisions")]
    private LayerMask groundMask;
    [SerializeField]
    [Tooltip("The colliders for the wheels of the cart")]
    private List<Collider> wheels;

    public GroundingModule(LayerMask groundMask, List<Collider> wheels)
    {
        this.groundMask = groundMask;
        this.wheels = wheels;
    }

    public bool Grounded(Transform parent)
    {
        Ray ray = new Ray(Vector3.zero, parent.up * -1);
        bool grnd = false;

        for(int i = 0; i < wheels.Count; i++)
        {
            ray.origin = wheels[i].bounds.center + (wheels[i].bounds.extents.y * parent.up * -1);
            grnd |= Physics.Raycast(ray, 0.1f);

            Debug.DrawRay(ray.origin, ray.direction);
        }

        return grnd;
    }
}
