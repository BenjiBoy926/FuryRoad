using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelectionAnimation : MonoBehaviour
{
    private Vector3 initialPosition;
    [SerializeField] private Vector3 finalPosition;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, finalPosition, 0.1f);
    }

    private void OnDisable()
    {
        transform.position = initialPosition;
    }
}
