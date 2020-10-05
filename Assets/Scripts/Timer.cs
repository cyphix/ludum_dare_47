using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Timer : MonoBehaviour
{
    public int minutesLength = 20; // bug if below number of phases
    public int numberOfPhases = 10;
    public Text debugTimerDisplay;
    public IntersceneObject intersceneObject;
    public string endMenuScene;

    private float timeRemaining = 0.0f;
    private int currentPhase = 0;
    private float phaseLength = 0.0f;

    void Start()
    {
        this.currentPhase = this.numberOfPhases;
        this.phaseLength = (float)((float)this.minutesLength * 60.0f / (float)this.numberOfPhases);

        this.ResetTimer();
    }
    
    void Update()
    {
        // update the timer
        if(this.timeRemaining > 0)
        {
            this.timeRemaining -= Time.deltaTime;
        }
        else
        {
            this.Doomsday();
        }

        // update the phase
        this.currentPhase = Mathf.CeilToInt(this.timeRemaining / this.phaseLength);

        // make readable
        float minutes = Mathf.FloorToInt(this.timeRemaining / 60);
        float seconds = Mathf.FloorToInt(this.timeRemaining % 60);
        this.debugTimerDisplay.text = "Timer: " + minutes + ":" + seconds + "\tPhase: " + this.currentPhase;
    }



    private void Doomsday()
    {
        this.intersceneObject.win = false;
        SceneManager.LoadScene(this.endMenuScene);
    }



    public void AddTime(int seconds)
    {
        if(this.timeRemaining + (float)seconds > (float)(this.minutesLength * 60))
        {
            this.ResetTimer();
        }
        else
        {
            this.timeRemaining += (float)seconds;
        }
    }

    public int GetPhase()
    {
        return this.currentPhase;
    }

    public void ResetTimer()
    {
        this.timeRemaining = (float)(this.minutesLength * 60);
    }
}
