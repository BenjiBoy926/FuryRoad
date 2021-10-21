using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEventHook : MonoBehaviour
{
    #region Public Typedefs
    [System.Serializable]
    public class CollisionEvent : UnityEvent<Collision> { }
    [System.Serializable]
    public class Collision2DEvent : UnityEvent<Collision2D> { }
    [System.Serializable]
    public class ColliderEvent : UnityEvent<Collider> { }
    [System.Serializable]
    public class Collider2DEvent : UnityEvent<Collider2D> { }
    #endregion

    #region Public Properties
    public UnityEvent<Collision> CollisionEnter => collisionEnter;
    public UnityEvent<Collision> CollisionStay => collisionStay;
    public UnityEvent<Collision> CollisionExit => collisionExit;

    public UnityEvent<Collision2D> CollisionEnter2D => collisionEnter2D;
    public UnityEvent<Collision2D> CollisionStay2D => collisionStay2D;
    public UnityEvent<Collision2D> CollisionExit2D => collisionExit2D;

    public UnityEvent<Collider> TriggerEnter => triggerEnter;
    public UnityEvent<Collider> TriggerStay => triggerStay;
    public UnityEvent<Collider> TriggerExit => triggerExit;

    public UnityEvent<Collider2D> TriggerEnter2D => triggerEnter2D;
    public UnityEvent<Collider2D> TriggerStay2D => triggerStay2D;
    public UnityEvent<Collider2D> TriggerExit2D => triggerExit2D;
    #endregion

    #region Public Events
    [Header("Collision 3D")]

    [SerializeField]
    private CollisionEvent collisionEnter;
    [SerializeField]
    private CollisionEvent collisionStay;
    [SerializeField]
    private CollisionEvent collisionExit;

    [Header("Collision 2D")]

    [SerializeField]
    private Collision2DEvent collisionEnter2D;
    [SerializeField]
    private Collision2DEvent collisionStay2D;
    [SerializeField]
    private Collision2DEvent collisionExit2D;

    [Header("Trigger 3D")]

    [SerializeField]
    private ColliderEvent triggerEnter;
    [SerializeField]
    private ColliderEvent triggerStay;
    [SerializeField]
    private ColliderEvent triggerExit;

    [Header("Trigger 2D")]

    [SerializeField]
    private Collider2DEvent triggerEnter2D;
    [SerializeField]
    private Collider2DEvent triggerStay2D;
    [SerializeField]
    private Collider2DEvent triggerExit2D;
    #endregion

    #region Monobehaviour Messages
    private void OnCollisionEnter(Collision collision) => collisionEnter.Invoke(collision);
    private void OnCollisionStay(Collision collision) => collisionStay.Invoke(collision);
    private void OnCollisionExit(Collision collision) => collisionExit.Invoke(collision);

    private void OnCollisionEnter2D(Collision2D collision) => collisionEnter2D.Invoke(collision);
    private void OnCollisionStay2D(Collision2D collision) => collisionStay2D.Invoke(collision);
    private void OnCollisionExit2D(Collision2D collision) => collisionExit2D.Invoke(collision);

    private void OnTriggerEnter(Collider other) => triggerEnter.Invoke(other);
    private void OnTriggerStay(Collider other) => triggerStay.Invoke(other);
    private void OnTriggerExit(Collider other) => triggerExit.Invoke(other);

    private void OnTriggerEnter2D(Collider2D collision) => triggerEnter2D.Invoke(collision);
    private void OnTriggerStay2D(Collider2D collision) => triggerStay2D.Invoke(collision);
    private void OnTriggerExit2D(Collider2D collision) => triggerExit2D.Invoke(collision);
    #endregion
}
