using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class NetworkLeaveRoomButton : MonoBehaviour
{
    [Header("Main control")]

    [SerializeField]
    [Tooltip("Reference to the main button")]
    private Button mainButton;
    [SerializeField]
    [Tooltip("Image on the main button")]
    private Image mainButtonImage;
    [SerializeField]
    [Tooltip("Text of the main button")]
    private Text mainButtonText;
    [SerializeField]
    [Tooltip("Color of the main button when confirmation is inactive")]
    private Color leaveColor;
    [SerializeField]
    [Tooltip("Color of the main button when confirmaiton is active")]
    private Color cancelColor;

    [Header("Additional controls")]

    [SerializeField]
    [Tooltip("Button that confirms that we want to leave the room")]
    private Button confirmButton;
    [SerializeField]
    [Tooltip("Button that cancels leaving the room")]
    private Button cancelButton;
    [SerializeField]
    [Tooltip("Base object for the confirmation panel")]
    private GameObject confirmPanelParent;

    // If we set the button to not be interactable, we have to make sure the panel is no longer visible
    public bool interactable
    {
        get => mainButton.interactable;
        set
        {
            mainButton.interactable = value;
            if (!value) SetConfirmPanelActive(false);
        }
    }

    private void Start()
    {
        mainButton.onClick.AddListener(ToggleConfirmPanel);
        confirmButton.onClick.AddListener(Leave);
        cancelButton.onClick.AddListener(() => SetConfirmPanelActive(false));

        SetConfirmPanelActive(false);
    }

    private void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void ToggleConfirmPanel()
    {
        // If it is active, make it inactive
        // If it is inactive, make it active
        SetConfirmPanelActive(!confirmPanelParent.activeInHierarchy);
    }

    private void SetConfirmPanelActive(bool active)
    {
        confirmPanelParent.SetActive(active);

        if(active)
        {
            mainButtonImage.color = cancelColor;
            mainButtonText.text = "Are you sure?";
        }
        else
        {
            mainButtonImage.color = leaveColor;
            mainButtonText.text = "Leave";
        }
    }
}
