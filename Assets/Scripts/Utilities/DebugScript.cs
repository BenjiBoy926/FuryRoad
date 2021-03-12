using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if(h > 0)
        {
            transform.rotation = Quaternion.AngleAxis(90f, Vector3.right);
        }
        if (h < 0)
        {
            transform.rotation = Quaternion.AngleAxis(0f, Vector3.left);
        }
        if(v > 0)
        {
            transform.rotation = Quaternion.AngleAxis(0f, Vector3.forward);
        }
        if(v < 0)
        {
            transform.rotation = Quaternion.AngleAxis(0f, Vector3.back);
        }
    }
}
