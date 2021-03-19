using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceCountdownGUIModule : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Text that displays 'Ready, set, GO!'")]
    private Text countdownText;
    [SerializeField]
    [Tooltip("List of the GUI objects that indicate each tick before the race starts")]
    private List<GameObject> tickMarkers;
    [SerializeField]
    [Tooltip("Each of these items shakes while the countdown says 'GO!'")]
    private List<ShakerModule> shakers;

    private int currentTick = 0;

    public void Advance()
    {
        // Check to make sure we are not advancing too far
        if(currentTick < tickMarkers.Count)
        {
            // Enable the next tick marker
            tickMarkers[currentTick].SetActive(true);
            currentTick++;

            // Play a sound

            // If the countdown is half way, change the text to say "Set"
            if(currentTick > tickMarkers.Count / 2)
            {
                countdownText.text = "Set...";
            }
            // If the countdown is finished, set the text to say GO! 
            // and the shakers to start shaking
            if(currentTick >= tickMarkers.Count)
            {
                countdownText.text = "GO!!!";

                // Player a different sound

                foreach (ShakerModule shaker in shakers)
                {
                    shaker.SetShakingActive(true);
                }
            }
        }
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);

        // If the countdown is activating, deactivate all tick markers to start off
        if(active)
        {
            foreach(GameObject marker in tickMarkers)
            {
                marker.SetActive(false);
            }
            foreach(ShakerModule shaker in shakers)
            {
                shaker.SetShakingActive(false);
            }
            currentTick = 0;
            countdownText.text = "Ready...";
        }
    }
}
