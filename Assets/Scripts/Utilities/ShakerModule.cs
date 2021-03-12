using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakerModule : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Interval between each position skip")]
    private float shakeInterval;
    [SerializeField]
    [Tooltip("Max distance from the origin that the object can move")]
    private float shakeMagnitude;

    private Vector3 origin; // The original position of the object
    private WaitForSeconds shakeWait;
    private bool isShaking = false; // True if the object is currently shaking
    private float timeSinceLastShake;   // Time that has elapsed since the last time the object shook

    // Start is called before the first frame update
    void Awake()
    {
        origin = transform.position;
        shakeWait = new WaitForSeconds(shakeInterval);
    }

    public void SetShakingActive(bool active)
    {
        isShaking = active;

        if(active)
        {
            timeSinceLastShake = shakeInterval;
        }
        else
        {
            transform.position = origin;
        }
    }

    private void Update()
    {
        if(isShaking)
        {
            // Update time since last shake
            timeSinceLastShake += Time.deltaTime;

            // If time since last shake exceeds time between shakes, then shake again
            if(timeSinceLastShake >= shakeInterval)
            {
                Shake();
                timeSinceLastShake = 0f;
            }
        }
    }

    private void Shake()
    {
        transform.position = origin + (Vector3)(Random.insideUnitCircle * shakeMagnitude);
    }
}
