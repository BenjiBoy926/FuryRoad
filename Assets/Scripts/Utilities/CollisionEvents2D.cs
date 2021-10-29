using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvents2D : MonoBehaviour
{
    #region Public Typedefs
    [System.Serializable]
    public class Collision2DEvent : UnityEvent<Collision2D> { }
    [System.Serializable]
    public class Collider2DEvent : UnityEvent<Collider2D> { }
    #endregion

    #region Public Properties
    public UnityEvent<Collision2D> CollisionEnter2D => collisionEnter2D;
    public UnityEvent<Collision2D> CollisionStay2D => collisionStay2D;
    public UnityEvent<Collision2D> CollisionExit2D => collisionExit2D;

    public UnityEvent<Collider2D> TriggerEnter2D => triggerEnter2D;
    public UnityEvent<Collider2D> TriggerStay2D => triggerStay2D;
    public UnityEvent<Collider2D> TriggerExit2D => triggerExit2D;
    #endregion

    #region Private Editor Fields
    [Header("Collision")]

    [SerializeField]
    private Collision2DEvent collisionEnter2D;
    [SerializeField]
    private Collision2DEvent collisionStay2D;
    [SerializeField]
    private Collision2DEvent collisionExit2D;

    [Header("Trigger")]

    [SerializeField]
    private Collider2DEvent triggerEnter2D;
    [SerializeField]
    private Collider2DEvent triggerStay2D;
    [SerializeField]
    private Collider2DEvent triggerExit2D;
    #endregion

    #region Monobehaviour Messages
    private void OnCollisionEnter2D(Collision2D collision) => collisionEnter2D.Invoke(collision);
    private void OnCollisionStay2D(Collision2D collision) => collisionStay2D.Invoke(collision);
    private void OnCollisionExit2D(Collision2D collision) => collisionExit2D.Invoke(collision);

    private void OnTriggerEnter2D(Collider2D collision) => triggerEnter2D.Invoke(collision);
    private void OnTriggerStay2D(Collider2D collision) => triggerStay2D.Invoke(collision);
    private void OnTriggerExit2D(Collider2D collision) => triggerExit2D.Invoke(collision);
    #endregion
}
