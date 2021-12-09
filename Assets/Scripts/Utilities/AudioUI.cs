using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioUI : MonoBehaviourSingleton<AudioUI>
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Audio mixer that outputs all audio")]
    private AudioMixer mixer;
    [SerializeField]
    [Tooltip("Toggle that mutes and unmutes the audio")]
    private Toggle muteToggle;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        muteToggle.onValueChanged.AddListener(Mute);
        Mute(muteToggle.isOn);
    }
    #endregion

    #region Initialize Methods
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize() => CreateInstance();
    #endregion

    #region Public Methods
    public void Mute(bool mute)
    {
        if (mute) mixer.SetFloat("MasterVolume", -100f);
        else mixer.ClearFloat("MasterVolume");
    }
    #endregion
}
