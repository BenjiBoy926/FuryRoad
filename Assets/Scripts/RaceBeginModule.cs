using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaceBeginModule : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the module that helps with the countdown GUI")]
    private RaceCountdownGUIModule countdownModule;
    [SerializeField]
    [Tooltip("The number of ticks before the race starts")]
    private int numTicks;
    [SerializeField]
    [Tooltip("Time between each tick leading up to the GO")]
    private float timeBetweenTicks;
    [SerializeField]
    [Tooltip("Time after the final tick before the countdown GUI goes away")]
    private float finalTickTime;
    [SerializeField]
    [Tooltip("Event raised as soon as the race begins")]
    private UnityEvent raceBegunEvent;

    private WaitForSeconds tickWait;

    private void Awake()
    {
        tickWait = new WaitForSeconds(timeBetweenTicks);
    }

    public void RaceBegin()
    {
        StartCoroutine(RaceBeginCoroutine());
    }

    private IEnumerator RaceBeginCoroutine()
    {
        // Disable player control and display the countdown
        NetworkHelper.localPlayerManager.EnableControl(false);
        SetCountdownGUIActive(true);

        // Advance the first countdown GUI
        AdvanceCountdownGUI();

        // Advance each tick
        for(int i = 0; i < numTicks - 1; i++)
        {
            yield return Tick();
        }

        // Enable player control and invoke the race begin event
        NetworkHelper.localPlayerManager.EnableControl(true);
        raceBegunEvent.Invoke();

        // Wait a few seconds, then disable the countdown gui
        yield return new WaitForSeconds(finalTickTime);
        SetCountdownGUIActive(false);
    }

    private IEnumerator Tick()
    {
        yield return tickWait;
        AdvanceCountdownGUI();
    }

    private void SetCountdownGUIActive(bool active)
    {
        // Setup countdown GUI
        countdownModule.SetActive(active);
    }

    private void AdvanceCountdownGUI()
    {
        countdownModule.Advance();
    }
}
