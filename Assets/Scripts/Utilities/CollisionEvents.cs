using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvents : MonoBehaviour
{
    #region Public Typedefs
    [System.Serializable]
    public class CollisionEvent : UnityEvent<Collision> { }
    [System.Serializable]
    public class ColliderEvent : UnityEvent<Collider> { }
    #endregion

    #region Public Properties
    public UnityEvent<Collision> CollisionEnter => collisionEnter;
    public UnityEvent<Collision> CollisionStay => collisionStay;
    public UnityEvent<Collision> CollisionExit => collisionExit;

    public UnityEvent<Collider> TriggerEnter => triggerEnter;
    public UnityEvent<Collider> TriggerStay => triggerStay;
    public UnityEvent<Collider> TriggerExit => triggerExit;
    #endregion

    #region Public Events
    [Header("Collision")]

    [SerializeField]
    private CollisionEvent collisionEnter;
    [SerializeField]
    private CollisionEvent collisionStay;
    [SerializeField]
    private CollisionEvent collisionExit;

    [Header("Trigger")]

    [SerializeField]
    private ColliderEvent triggerEnter;
    [SerializeField]
    private ColliderEvent triggerStay;
    [SerializeField]
    private ColliderEvent triggerExit;
    #endregion

    #region Monobehaviour Messages
    private void OnCollisionEnter(Collision collision) => collisionEnter.Invoke(collision);
    private void OnCollisionStay(Collision collision) => collisionStay.Invoke(collision);
    private void OnCollisionExit(Collision collision) => collisionExit.Invoke(collision);

    private void OnTriggerEnter(Collider other) => triggerEnter.Invoke(other);
    private void OnTriggerStay(Collider other) => triggerStay.Invoke(other);
    private void OnTriggerExit(Collider other) => triggerExit.Invoke(other);
    #endregion
}
