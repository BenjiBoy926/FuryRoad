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
        public string messageFormat;
        public Color color;
        public Vector2 startingAnchor;
        public Vector2 endingAnchor;
        public Ease animationEase;

        public void Animate(TextMeshProUGUI target, float time, int playerNumber)
        {
            // Kill any active tweens
            target.rectTransform.DOKill();

            // Setup the text
            target.gameObject.SetActive(true);
            target.text = string.Format(messageFormat, playerNumber);
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
        int playerNumber = -1;

        // Get the network player if the network is connected
        if (PhotonNetwork.IsConnected)
        {
            playerNumber = NetworkPlayer.GetPlayer(other.gameObject).ActorNumber;
        }

        // Animate the text
        AnimateProjectileHitOther(playerNumber);
    }
    private void OnProjectileHitMe(Projectile projectile)
    {
        int otherPlayer = -1;

        // Get the owner of the projectile if they exist
        if (PhotonNetwork.IsConnected)
        {
            PhotonView projectileView = projectile.GetComponent<PhotonView>();

            // Set other player to the owner of the photon view
            if (projectileView) otherPlayer = projectileView.OwnerActorNr;
        }

        // Animate the text
        AnimateProjectileHitMe(otherPlayer);
    }
    #endregion

    #region Public Methods
    public void AnimateProjectileHitOther(int otherActorNumber)
    {
        projectileHitOtherAnimation.Animate(text, animationTime, otherActorNumber);
    }
    public void AnimateProjectileHitMe(int projectileActorNumber)
    {
        projectileHitMeAnimation.Animate(text, animationTime, projectileActorNumber);
    }
    #endregion
}
