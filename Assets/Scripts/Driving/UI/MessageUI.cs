using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;
using DG.Tweening;

public class MessageUI : DrivingModule
{
    #region Public Typedefs
    [System.Serializable]
    public struct TextAnimationData
    {
        public Color color;
        public Vector2 startingAnchor;
        public Vector2 endingAnchor;
        public Ease animationEase;

        public void Animate(TextMeshProUGUI target, float time, string text)
        {
            // Kill any active tweens
            target.rectTransform.DOKill();

            // Setup the text
            target.gameObject.SetActive(true);
            target.text = text;
            target.color = color;
            target.rectTransform.anchoredPosition = startingAnchor;

            // Move the target to the ending position, then disable it
            target.rectTransform.DOAnchorPos(endingAnchor, time)
                .SetEase(animationEase)
                .OnComplete(() => target.gameObject.SetActive(false));
        }
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Text that displays when projectiles hit me, or when my projectile hits someone")]
    private TextMeshProUGUI text;
    [SerializeField]
    [Tooltip("Time it takes to animate the text")]
    private float animationTime = 0.5f;
    [SerializeField]
    [Tooltip("Animation used when my projectile hits another driver")]
    [FormerlySerializedAs("projectileHitOtherAnimation")]
    private TextAnimationData positiveAnimation;
    [SerializeField]
    [Tooltip("Animation used when another projectile hits me")]
    [FormerlySerializedAs("projectileHitMeAnimation")]
    private TextAnimationData negativeAnimation;
    #endregion

    #region Monobehaviour Messages
    protected override void Start()
    {
        base.Start();

        // Disable the text
        text.gameObject.SetActive(false);

        // Subscribe to projectile events
        manager.ProjectileHitOtherEvent.AddListener(OnProjectileHitOther);
        manager.ProjectileHitMeEvent.AddListener(OnProjectileHitMe);
    }
    #endregion

    #region Event Listeners
    private void OnProjectileHitOther(DrivingManager other)
    {
        ProjectileHitOtherMessage(other);
    }
    private void OnProjectileHitMe(Projectile projectile)
    {
        ProjectileHitMeMessage(projectile.OwningDriver);
    }
    #endregion

    #region Public Methods
    public void PositiveMessage(string message)
    {
        positiveAnimation.Animate(text, animationTime, message);
    }
    public void NegativeMessage(string message)
    {
        negativeAnimation.Animate(text, animationTime, message);
    }
    public void ProjectileHitOtherMessage(DrivingManager otherDriver)
    {
        if (otherDriver != manager)
        {
            PositiveMessage($"You hit {otherDriver.ID}!");
        }
        else NegativeMessage($"You hit yourself...");
    }
    public void ProjectileHitMeMessage(DrivingManager otherDriver)
    {
        if (otherDriver.driverNumber.Invoke() != manager.driverNumber.Invoke())
        {
            NegativeMessage($"{otherDriver.ID} hit you...");
        }
        else NegativeMessage("You hit yourself...");
    }
    #endregion
}
