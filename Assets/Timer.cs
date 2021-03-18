using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    [SerializeField]
    [Tooltip("Start time for the timer")]
    public float startTime;
    [SerializeField]
    [Tooltip("Elapsed time for the timer")]
    public float elapsedTime;
    private bool finishedRace;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(finishedRace){
            return;
        }
        elapsedTime = Time.time - startTime;
        string minutes = ((int) elapsedTime / 60).ToString();
        string seconds = (elapsedTime % 60).ToString("f2");
        
        timerText.text = minutes + ":" + seconds;
    }

    public void Finished(){
        finishedRace = true;
        timerText.color = Color.green;
    }
}
