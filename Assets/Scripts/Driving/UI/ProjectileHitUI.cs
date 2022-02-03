using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Photon.Pun;

public class ProjectileHitUI : DrivingModule
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
    private TextAnimationData projectileHitOtherAnimation;
    [SerializeField]
    [Tooltip("Animation used when another projectile hits me")]
    private TextAnimationData projectileHitMeAnimation;
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
        // Animate the text
        AnimateProjectileHitOther(other.ID);
    }
    private void OnProjectileHitMe(Projectile projectile)
    {
        string projectileOwnerName = "P0";

        // If the projectile has an owning driver then the name is their name
        if (projectile.OwningDriver)
        {
            projectileOwnerName = projectile.OwningDriver.ID;
        }

        // Animate the text
        AnimateProjectileHitMe(projectileOwnerName);
    }
    #endregion

    #region Public Methods
    public void AnimateProjectileHitOther(string otherActorName)
    {
        string content = $"You hit {otherActorName}!";
        projectileHitOtherAnimation.Animate(text, animationTime, content);
    }
    public void AnimateProjectileHitMe(string projectileActorName)
    {
        string content = $"{projectileActorName} hit you...";
        projectileHitMeAnimation.Animate(text, animationTime, content);
    }
    #endregion
}
